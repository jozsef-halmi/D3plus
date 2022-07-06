using Catalog.Domain.Common;

namespace Catalog.Application.Outbox;

public class OutboxMessage : BaseEntity
{
    public string IntegrationEventType { get; set; }
    public string IntegrationEventJson { get; set; }
    public DateTime CreatedDate => DateTime.UtcNow;
    public DateTime? PublishedDate { get; set; }
}
