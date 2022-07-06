using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Events;
using MediatR;
using Messaging.Contracts;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Product.EventHandlers;

public class ProductDeletedEventHandler : INotificationHandler<ProductDeletedEvent>
{
    private readonly ILogger<ProductDeletedEventHandler> _logger;
    private readonly IIntegrationEventService _integrationEventService;

    public ProductDeletedEventHandler(IIntegrationEventService integrationEventService, ILogger<ProductDeletedEventHandler> logger)
    {
        _logger = logger;
        _integrationEventService = integrationEventService;
    }

    public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);

        await _integrationEventService.Publish(new ProductDeletedIntegrationEvent(notification.Product.Id));
    }
}
