using Catalog.GraphQL.GraphQL.Types;
using GraphQL;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class ProductsMutation : ObjectGraphType
{
    /// <example>
    /// This is an example JSON request for a mutation
    /// {
    ///   "query": "mutation ($product:ProductInput!){ createProduct(product: $product) { id name } }",
    ///   "variables": {
    ///     "product": {
    ///       "name": "Paper"
    ///     }
    ///   }
    /// }
    /// </example>
    public ProductsMutation(IServiceProvider serviceProvider)
    {
        Name = "Mutation";

        FieldAsync<ProductType>(
            "createProduct",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<CreateProductInputType>> { Name = "product" }
            ),
            resolve: async context => 
            {
                var product = context.GetArgument<Product>("product");
                using var scope = serviceProvider.CreateScope();
                var services = scope.ServiceProvider;
                var productsData = services.GetRequiredService<ProductsDataLoader>();
                return await productsData.AddProduct(product);
            });

        FieldAsync<ProductType>(
           "updateProduct",
           arguments: new QueryArguments(
               new QueryArgument<NonNullGraphType<UpdateProductInputType>> { Name = "product" }
           ),
           resolve: async context =>
           {
               var product = context.GetArgument<Product>("product");
               using var scope = serviceProvider.CreateScope();
               var services = scope.ServiceProvider;
               var productsData = services.GetRequiredService<ProductsDataLoader>();
               return await productsData.UpdateProduct(product);
           });


        FieldAsync<ProductType>(
           "deleteProduct",
           arguments: new QueryArguments(
               new QueryArgument<NonNullGraphType<DeleteProductInputType>> { Name = "product" }
           ),
           resolve: async context =>
           {
               var product = context.GetArgument<Product>("product");
               using var scope = serviceProvider.CreateScope();
               var services = scope.ServiceProvider;
               var productsData = services.GetRequiredService<ProductsDataLoader>();
               return await productsData.DeleteProduct(product);
           });
    }
}
