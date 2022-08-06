namespace Messaging.Contracts;

public class ProductPriceChangedIntegrationEvent : IntegrationEvent
{
    public int ProductId { get; private init; }

    public decimal NewPrice { get; private init; }

    public decimal OldPrice { get; private init; }

    public ProductPriceChangedIntegrationEvent(int productId, decimal newPrice, decimal oldPrice, string traceRootId, string traceParentId) 
    {
        ProductId = productId;
        NewPrice = newPrice;
        OldPrice = oldPrice;
        TraceParentId = traceParentId;
        TraceRootId = traceRootId;
    }
}
