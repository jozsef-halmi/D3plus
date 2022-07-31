using Catalog.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using Infrastructure.GraphQL;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllers()
            .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddGraphQL(b => b
            .AddHttpMiddleware<ISchema>()
            //.AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User })
            .AddSystemTextJson()
            .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
            .AddSchema<CategoriesSchema>()
            .AddGraphTypes(typeof(CategoriesQuery).Assembly));
        services.AddSingleton<CategoriesData>();

        services.AddLogging(builder => builder.AddConsole());
        services.AddHttpContextAccessor();

        return services;
    }
}
