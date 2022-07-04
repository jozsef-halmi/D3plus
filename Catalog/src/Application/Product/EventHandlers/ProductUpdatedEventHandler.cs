using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Events;
using MediatR;
using Messaging.Contracts;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Product.EventHandlers;

public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
{
    private readonly ILogger<ProductUpdatedEventHandler> _logger;
    private readonly IIntegrationEventService _integrationEventService;

    public ProductUpdatedEventHandler(ILogger<ProductUpdatedEventHandler> logger, IIntegrationEventService integrationEventService)
    {
        _logger = logger;
        _integrationEventService = integrationEventService;
    }

    public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);

        if (notification.OldProduct.Price != notification.NewProduct.Price)
        {
            await _integrationEventService.Publish(new ProductPriceChangedIntegrationEvent(notification.NewProduct.Id, notification.NewProduct.Price, notification.OldProduct.Price));
        }
    }
}
