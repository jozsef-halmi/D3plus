namespace Messaging.Contracts;

public class ProductDeletedIntegrationEvent : IntegrationEvent
{
    public int ProductId { get; private init; }

    public ProductDeletedIntegrationEvent(int productId, string traceRootId, string traceParentId)
    {
        ProductId = productId;
        TraceParentId = traceParentId;
        TraceRootId = traceRootId;
    }
}
