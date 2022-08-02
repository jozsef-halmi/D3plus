using System.Reflection;
using Catalog.GraphQL.GraphQL;
using Catalog.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Catalog.GraphQL;

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
            .AddHttpMiddleware<CategoriesSchema>()
            .AddHttpMiddleware<ProductsSchema>()
            .AddSystemTextJson()
            .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
            .AddSchema<CategoriesSchema>()
            .AddSchema<ProductsSchema>()
            .AddDataLoader()
            .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
            .AddGraphTypes(typeof(CategoriesQuery).Assembly));

        services.AddScoped<CategoriesDataLoader>();
        services.AddScoped<ProductsDataLoader>();

        services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
        services.AddSingleton<DataLoaderDocumentListener>();

        services.AddSingleton<ProductsSchema>();

        services.AddLogging(builder => builder.AddConsole());
        services.AddHttpContextAccessor();

        return services;
    }
}
