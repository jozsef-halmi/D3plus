using Catalog.Domain.Entities;

namespace Catalog.Domain.Events;

public class ProductUpdatedEvent : BaseEvent
{
    public ProductUpdatedEvent(Product oldProduct, Product newProduct)
    {
        OldProduct = oldProduct;
        NewProduct = newProduct;
    }

    public Product OldProduct { get; init; }
    public Product NewProduct { get; init; }

}
