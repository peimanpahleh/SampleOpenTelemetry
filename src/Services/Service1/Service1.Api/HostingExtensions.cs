
namespace Service1.Api;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MySettings>(builder.Configuration.GetSection(nameof(MySettings)));

        #region Default Metric Lable
        Metrics.DefaultRegistry.SetStaticLabels(new Dictionary<string, string>
        {
          // Labels applied to all metrics in the registry.
          { "servicename", "service1" }
        });
        #endregion

        builder.Services.AddScoped<IEventBus, MassTransitEventBus>();


        #region Add Grpc
        var settings = builder.Configuration.GetSection(nameof(MySettings))
                                         .Get<MySettings>();

        if (settings == null)
            throw new InvalidOperationException("Service2Url is null");

        builder.Services.AddGrpcClient<GreeterClient>(options =>
        {
            options.Address = new Uri(settings.Service2Url);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            return handler;
        });
        #endregion

        #region Add DbContext
        var psqlConnectionString = builder.Configuration.GetConnectionString("Psql"); 
        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString: psqlConnectionString);
        });
        #endregion

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        #region Add Grpc
        //builder.Services.AddGrpc();
        #endregion




        #region Add MassTransit
        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UsePrometheusMetrics(serviceName: "mt_service1");
                cfg.ConfigureEndpoints(context);
            });
        });
        builder.Services.AddMassTransitHostedService(true);
        #endregion

        #region Add OpenTelemetry Tracing
        builder.Services.AddOpenTelemetryTracing(b =>
        {
            b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Service1"));
            b.AddAspNetCoreInstrumentation();
            b.AddHttpClientInstrumentation();
            b.AddNpgsql();
            b.AddMassTransitInstrumentation();

            b.AddZipkinExporter();
        });
        #endregion

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();

        app.MapControllers();

        #region promethues-dotnet Metrics

        app.UseMetricServer();
        app.UseHttpMetrics();
        app.UseGrpcMetrics();

        #endregion

        return app;
    }

}
