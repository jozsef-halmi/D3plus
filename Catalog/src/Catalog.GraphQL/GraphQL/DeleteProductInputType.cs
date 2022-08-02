using Catalog.GraphQL.GraphQL.Types;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class DeleteProductInputType : InputObjectGraphType<Product>
{
    public DeleteProductInputType()
    {
        Name = "DeleteProductInput";
        Field(x => x.Id, nullable: true);
    }
}