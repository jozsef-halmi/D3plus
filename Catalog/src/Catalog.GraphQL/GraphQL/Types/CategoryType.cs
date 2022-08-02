using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL.Types;

public class CategoryType : ObjectGraphType<Category>
{
    public CategoryType()
    {
        Name = "Category";
        Description = "A category of a product";

        Field(d => d.Id, nullable: false).Description("The id of the category.");
        Field(d => d.Name, nullable: true).Description("The description of the category.");
        Field(d => d.ParentCategoryId, nullable: true).Description("The id of the parent category.");
        Field(d => d.ParentCategoryName, nullable: true).Description("The name of the parent category.");
        Field(d => d.ImageUrl, nullable: true).Description("The url of the image.");
    }
}

