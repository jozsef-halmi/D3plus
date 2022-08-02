using GraphQL.DataLoader;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL.Types;

public class ProductType : ObjectGraphType<Product>
{
    public ProductType(IServiceProvider serviceProvider, IDataLoaderContextAccessor dataLoaderContextAccessor)
    {
        Name = "Product";
        Description = "A product";

        Field(d => d.Id, nullable: false).Description("The id of the product.");
        Field(d => d.Name, nullable: false).Description("The name of the product.");
        Field(d => d.Description, nullable: true).Description("The description of the product.");
        Field(d => d.ImageUrl, nullable: true).Description("The url of the image.");
        Field(d => d.CategoryId, nullable: false).Description("The id of the associated category");
        Field(d => d.CategoryName, nullable: false).Description("The name of the associated category");
        Field(d => d.Price, nullable: false).Description("The price of the product.");
        Field(d => d.Amount, nullable: false).Description("The amount of thep roduct.");

        FieldAsync<CategoryType>(
            "category",
            resolve: async context =>
            {
                using var scope = serviceProvider.CreateScope();
                var services = scope.ServiceProvider;
                var productsDataLoader = services.GetRequiredService<ProductsDataLoader>();
                // Get or add a batch loader with the key "GetCategoriesById"
                // The loader will call GetCategoriesByIdAsync for each batch of keys

                var loader = dataLoaderContextAccessor.Context.GetOrAddBatchLoader<int, Category>("GetCategoriesById", async ids =>
                {
                    //create the scope inside the data loader(need a reference to any IServiceProvider to use CreateScope)
                    using var scope = context.RequestServices.CreateScope();

                    //grab the service provider from the scope
                    var serviceProvider = scope.ServiceProvider;

                    //grab the data loader from the scope
                    var productsDataLoader = serviceProvider.GetRequiredService<ProductsDataLoader>();

                    return await productsDataLoader.GetCategoriesByIdAsync(ids, CancellationToken.None);
                });

                // Add this CategoryId to the pending keys to fetch
                // The execution strategy will trigger the data loader to fetch the data via GetCategoriesByIdAsync() at the
                //   appropriate time, and the field will be resolved with an instance of User once GetCategoriesByIdAsync()
                //   returns with the batched results
                return loader.LoadAsync(context.Source.CategoryId);
            }
        );
    }
}

