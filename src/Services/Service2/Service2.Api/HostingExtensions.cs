
namespace Service2.Api;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        #region Default Metric Lable

        Metrics.DefaultRegistry.SetStaticLabels(new Dictionary<string, string>
        {
          // Labels applied to all metrics in the registry.
          { "servicename", "service2" }
        });

        #endregion


        builder.Services.AddScoped<IEventBus, MassTransitEventBus>();


        #region Add Grpc
        builder.Services.AddGrpc();
        #endregion

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();





        #region Add MassTransit
        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<TestMsgHandler>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UsePrometheusMetrics(serviceName: "mt_service2");
                cfg.ConfigureEndpoints(context);
            });
        });
        builder.Services.AddMassTransitHostedService(true);
        #endregion

        #region Add OpenTelemetry Tracing
        builder.Services.AddOpenTelemetryTracing(b =>
        {
            b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Service2"));
            b.AddAspNetCoreInstrumentation();
            b.AddHttpClientInstrumentation();
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

        app.MapGrpcService<GreeterService>();


        #region promethues-dotnet Metrics

        app.UseMetricServer();
        app.UseHttpMetrics();
        app.UseGrpcMetrics();

        #endregion

        return app;
    }

}
