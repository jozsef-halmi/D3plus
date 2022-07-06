using Catalog.Application.Common.Interfaces;
using Catalog.Application.Outbox;
using Catalog.Domain.Events;
using MediatR;
using Messaging.Contracts;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Product.EventHandlers;

public class ProductDeletedEventHandler : INotificationHandler<ProductDeletedEvent>
{
    private readonly ILogger<ProductDeletedEventHandler> _logger;
    private readonly IApplicationDbContext _dbContext;

    public ProductDeletedEventHandler(IApplicationDbContext dbContext, ILogger<ProductDeletedEventHandler> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);

            var integrationEvent = new ProductDeletedIntegrationEvent(notification.Product.Id);

            _dbContext.OutboxMessages.Add(new OutboxMessage()
            {
                PublishedDate = null,
                IntegrationEventType = integrationEvent.GetType().FullName,
                IntegrationEventJson = System.Text.Json.JsonSerializer.Serialize(integrationEvent)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while saving outbox message ProductDeletedEvent. {Message}", ex.Message);
            throw;
        }
    }
}
