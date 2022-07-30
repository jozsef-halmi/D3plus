using ApiGateway.Aggregators;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Cache.CacheManager;

namespace ApiGateway
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            //services.AddSwaggerGen();
            services.AddSwaggerForOcelot(configuration);

            services.AddOcelot()
                .AddSingletonDefinedAggregator<GetProductAggregator>()
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                });

            services.AddGatewayAuthentication(configuration);

            return services;
        }

        public static IServiceCollection AddGatewayAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var identityUrl = configuration.GetSection("Identity").GetValue<string>("Url");

            // Accepts any access token issued by identity server
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = identityUrl;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            // Adds an authorization policy to make sure the token is for scope 'catalog'
            services.AddAuthorization(options =>
            {
            });

            return services;
        }
    }
}
