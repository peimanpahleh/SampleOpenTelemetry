namespace Products.Api.Settings;

public class ServiceSettings
{
    public string MongoDbConnection { get; set; }
    public string PsqlConnection { get; set; }
    public MassTransitSettings MassTransitSettings { get; set; }

}
