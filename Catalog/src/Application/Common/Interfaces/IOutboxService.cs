namespace Catalog.Application.Outbox;

public interface IOutboxService
{
    Task ProcessMessages(CancellationToken cancellationToken);
}
