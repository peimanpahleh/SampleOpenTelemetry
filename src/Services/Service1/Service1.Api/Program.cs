
Log.Logger = new LoggerConfiguration()
    .CreateLogger();


Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);


    builder.Host.UseSerilog((context, cfg) =>
    {
        //cfg.MinimumLevel.Information();
        cfg.ReadFrom.Configuration(context.Configuration);
        cfg.Enrich.FromLogContext();
        cfg.Enrich.WithSpan();
        cfg.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code);
        cfg.WriteTo.Seq(serverUrl: "http://localhost:5341");
        cfg.ReadFrom.Configuration(context.Configuration);
    });


    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();


    // for testing purpose
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException")
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
