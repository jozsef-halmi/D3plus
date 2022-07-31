namespace Catalog.GraphQL.GraphQL.Types;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Uri? ImageUrl { get; set; }

    public int? ParentCategoryId { get; set; }

    public Category? ParentCategory { get; set; }
}