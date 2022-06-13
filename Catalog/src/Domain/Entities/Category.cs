namespace Catalog.Domain.Entities;

public class Category : BaseAuditableEntity
{
    public string Name { get; set; }

    public Uri? ImageUrl { get; set; }

    public int? ParentCategoryId { get; set; }

    public Category? ParentCategory { get; set; }
}
