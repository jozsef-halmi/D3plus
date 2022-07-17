namespace Catalog.Application.Common.Configuration;

public class MassTransitConfiguration
{
    public const string MassTransitConfigurationKey = "MassTransit";

    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }

}
