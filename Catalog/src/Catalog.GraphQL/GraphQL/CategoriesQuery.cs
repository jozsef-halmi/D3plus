using Catalog.GraphQL.GraphQL.Types;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class CategoriesQuery : ObjectGraphType<object>
{
    public CategoriesQuery(IServiceProvider serviceProvider)
    {
        Name = "Query";

        FieldAsync<ListGraphType<CategoryType>>("categories", resolve: async context =>
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            return await services.GetRequiredService<CategoriesDataLoader>().GetCategories();
        });

    }
}