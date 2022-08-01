using Catalog.GraphQL.GraphQL.Types;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class UpdateCategoryInputType : InputObjectGraphType<Category>
{
    public UpdateCategoryInputType()
    {
        Name = "UpdateCategoryInput";
        Field(x => x.Id, nullable: true);
        Field(x => x.Name, nullable: false);
        Field(x => x.ImageUrl, nullable: true);
        Field(x => x.ParentCategoryId, nullable: true);
    }
}