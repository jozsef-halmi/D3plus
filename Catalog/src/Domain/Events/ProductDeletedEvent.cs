using Catalog.Domain.Entities;

namespace Catalog.Domain.Events;

public class ProductDeletedEvent : BaseEvent
{
    public ProductDeletedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; init; }
}
