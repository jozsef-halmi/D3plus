using Catalog.Domain.Entities;

namespace Catalog.Domain.Events;

public class ProductUpdatedEvent : BaseEvent
{
    public ProductUpdatedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; init; }
}
