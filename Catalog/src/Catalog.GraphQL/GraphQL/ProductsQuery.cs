using Catalog.GraphQL.GraphQL.Types;
using GraphQL;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class ProductsQuery : ObjectGraphType<object>
{
    public ProductsQuery(IServiceProvider serviceProvider)
    {
        Name = "Query";

        FieldAsync<ListGraphType<ProductType>>("products",
             arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "categoryId", Description = "id of the category" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "pageNumber", Description = "which page is requested" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "pageSize", Description = "the size of the page" }
                ),

            resolve: async context =>
            {
                using var scope = serviceProvider.CreateScope();
                var services = scope.ServiceProvider;
                return await services.GetRequiredService<ProductsDataLoader>().GetProducts(
                    context.GetArgument<int>("categoryId"),
                    context.GetArgument<int>("pageNumber"),
                    context.GetArgument<int>("pageSize"));
            });
    }
}