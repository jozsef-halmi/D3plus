namespace Messaging.Contracts;

public class ProductDeletedIntegrationEvent : IntegrationEvent
{
    public int ProductId { get; private init; }

    public ProductDeletedIntegrationEvent(int productId)
    {
        ProductId = productId;
    }
}
