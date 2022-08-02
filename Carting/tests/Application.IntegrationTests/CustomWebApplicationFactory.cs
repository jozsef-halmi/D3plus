using Carting.WebApi.Application.Common.Configuration;
using Carting.WebApi.Application.Common.Interfaces;
using Carting.WebApi.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carting.Application.IntegrationTests;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            var persistenceConfiguration = new PersistenceOptions();
            integrationConfig.GetSection(nameof(PersistenceOptions)).Bind(persistenceConfiguration);
            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureServices((builder, services) =>
        {
            services.AddScoped<ICartingDbContext, CartingDbContext>();
            services.Configure<PersistenceOptions>(builder.Configuration.GetSection("PersistenceConfiguration"));
        });
    }
}
