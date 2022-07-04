namespace Catalog.Application.Common.Interfaces;

public interface IIntegrationEventService
{
    public Task Publish<T>(T message);
}
