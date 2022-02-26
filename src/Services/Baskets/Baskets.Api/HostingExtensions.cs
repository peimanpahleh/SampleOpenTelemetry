namespace Baskets.Api;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection(nameof(ServiceSettings)));

        #region Default Metric Lable
        Metrics.DefaultRegistry.SetStaticLabels(new Dictionary<string, string>
        {
          // Labels applied to all metrics in the registry.
          { "servicename", "basketsapi" }
        });
        #endregion

        var settings = GetServiceSettings(builder.Configuration);

        builder.Services.AddScoped<IEventBus, MassTransitEventBus>();

        builder.Services
            .AddCustomMassTransit(settings)
            .AddCustomOpenTelemetryTracing()
            .AddCustomCors();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        return builder.Build();
    }


    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseCors("CorsPolicy");

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        #region promethues-dotnet Metrics

        app.UseMetricServer();
        app.UseHttpMetrics();
        app.UseGrpcMetrics();

        #endregion

        app.MapGet("/", () =>
        {
            return "Welcome to baskets";
        });

        return app;
    }


    private static ServiceSettings GetServiceSettings(IConfiguration configuration)
    {
        return configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
    }

    private static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                .SetIsOriginAllowed((host) => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });
        return services;
    }

    private static IServiceCollection AddCustomMassTransit(this IServiceCollection services, ServiceSettings settings)
    {

        var mtSettings = settings.MassTransitSettings;

        if (mtSettings?.RabbitmqHost == null)
            throw new InvalidOperationException("MassTransitSettings is null");


        services.AddMassTransit(x =>
        {
            x.ApplyCustomMassTransitConfiguration();

            x.AddConsumersFromNamespaceContaining<ProductPriceChangedHandler>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UsePrometheusMetrics(serviceName: "mt_basketsapi");

                cfg.Host(mtSettings.RabbitmqHost, mtSettings.RabbitmqVirtualHost, h =>
                {
                    h.Username(mtSettings.RabbitmqUsername);
                    h.Password(mtSettings.RabbitmqPassword);
                });

                cfg.ApplyCustomBusConfiguration();

                cfg.PrefetchCount = 20;

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddMassTransitHostedService(true);


        return services;
    }

    private static IServiceCollection AddCustomOpenTelemetryTracing(this IServiceCollection services)
    {
        services.AddOpenTelemetryTracing(b =>
        {
            b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("BasketsApi"));
            b.AddAspNetCoreInstrumentation();
            b.AddHttpClientInstrumentation();
            b.AddNpgsql();
            b.AddMassTransitInstrumentation();

            b.AddZipkinExporter();
        });

        return services;
    }


}
