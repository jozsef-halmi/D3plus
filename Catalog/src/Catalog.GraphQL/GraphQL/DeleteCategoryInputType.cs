using Catalog.GraphQL.GraphQL.Types;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class DeleteCategoryInputType : InputObjectGraphType<Category>
{
    public DeleteCategoryInputType()
    {
        Name = "DeleteCategoryInput";
        Field(x => x.Id, nullable: true);
    }
}