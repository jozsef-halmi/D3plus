using Catalog.Infrastructure.Persistence;
using Catalog.WebApi.Filters;
using Catalog.WebApi.HostedServices;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllers(options =>
          options.Filters.Add<ApiExceptionFilterAttribute>())
              .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddHostedService<OutboxHostedService>();

        services.AddSwaggerGen();
        return services;
    }
}
