using Catalog.Application.Common.Interfaces;
using Catalog.Application.Outbox;
using Catalog.Domain.Events;
using MediatR;
using Messaging.Contracts;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Product.EventHandlers;

public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
{
    private readonly ILogger<ProductUpdatedEventHandler> _logger;
    private readonly IApplicationDbContext _dbContext;

    public ProductUpdatedEventHandler(ILogger<ProductUpdatedEventHandler> logger, IApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);

            if (notification.OldProduct.Price == notification.NewProduct.Price)
                return;

            var integrationEvent = new ProductPriceChangedIntegrationEvent(notification.NewProduct.Id, notification.NewProduct.Price, notification.OldProduct.Price);

            _dbContext.OutboxMessages.Add(new OutboxMessage()
            {
                PublishedDate = null,
                IntegrationEventType = integrationEvent.GetType().FullName,
                IntegrationEventJson = System.Text.Json.JsonSerializer.Serialize(integrationEvent)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while saving outbox message ProductUpdatedEvent. {Message}", ex.Message);
            throw;
        }
    }
}
