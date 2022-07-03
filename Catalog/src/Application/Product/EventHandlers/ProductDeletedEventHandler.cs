using Catalog.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Product.EventHandlers;

public class ProductDeletedEventHandler : INotificationHandler<ProductDeletedEvent>
{
    private readonly ILogger<ProductDeletedEventHandler> _logger;

    public ProductDeletedEventHandler(ILogger<ProductDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
