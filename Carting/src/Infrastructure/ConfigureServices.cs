using Carting.Application.Common.Interfaces;
using Carting.Infrastructure.Persistence;
using Carting.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICartingDbContext, CartingDbContext>();
        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}
