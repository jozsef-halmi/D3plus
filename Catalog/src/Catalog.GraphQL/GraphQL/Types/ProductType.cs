using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL.Types;

public class ProductType : ObjectGraphType<Product>
{
    public ProductType(IServiceProvider serviceProvider)
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
            resolve: async context => {
                using var scope = serviceProvider.CreateScope();
                var services = scope.ServiceProvider;
                return await services.GetRequiredService<ProductsData>().GetCategory(context.Source);
                }
        );
        //Field<ListGraphType<EpisodeEnum>>("appearsIn", "Which movie they appear in.");
        //Field(d => d.PrimaryFunction, nullable: true).Description("The primary function of the droid.");

        //Interface<CharacterInterface>();
    }
}

