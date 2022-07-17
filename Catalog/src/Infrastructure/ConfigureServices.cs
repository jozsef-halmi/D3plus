using Catalog.Application.Common.Configuration;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Outbox;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Persistence.Interceptors;
using Catalog.Infrastructure.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("CatalogDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddSingleton<IDateTime, DateTimeService>();
        services.AddScoped<IIntegrationEventService, IntegrationEventService>();

        services.AddMessaging(configuration);

        return services;
    }

    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var massTransitConfiguration = configuration.GetSection(MassTransitConfiguration.MassTransitConfigurationKey).Get<MassTransitConfiguration>();
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(massTransitConfiguration.Host, massTransitConfiguration.VirtualHost, h => {
                    h.Username(massTransitConfiguration.Username);
                    h.Password(massTransitConfiguration.Password);
                    h.RequestedConnectionTimeout(5000);
                });

                cfg.UseMessageRetry(r => r.Immediate(3));

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
