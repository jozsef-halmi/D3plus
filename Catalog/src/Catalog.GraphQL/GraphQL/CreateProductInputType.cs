using Catalog.GraphQL.GraphQL.Types;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class CreateProductInputType : InputObjectGraphType<Product>
{
    public CreateProductInputType()
    {
        Name = "CreateProductInput";
        Field(x => x.Id, nullable: true);
        Field(x => x.Name, nullable: false);
        Field(x => x.Description, nullable: true);
        Field(x => x.ImageUrl, nullable: true);
        Field(x => x.CategoryId, nullable: false);
        Field(x => x.Price, nullable: false);
        Field(x => x.Amount, nullable: false);
    }
}
