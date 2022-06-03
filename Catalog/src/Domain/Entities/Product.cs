namespace Catalog.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public Uri? ImageUrl { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public decimal Price { get; set; }

    public int Amount { get; set; }
}
