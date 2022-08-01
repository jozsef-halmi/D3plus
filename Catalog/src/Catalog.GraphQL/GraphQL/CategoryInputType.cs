using Catalog.GraphQL.GraphQL.Types;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class CategoryInputType : InputObjectGraphType<Category>
{
    public CategoryInputType()
    {
        Name = "CategoryInput";
        Field(x => x.Name, nullable: false);
        Field(x => x.ImageUrl, nullable: true);
        Field(x => x.ParentCategoryId, nullable: true);
    }
}