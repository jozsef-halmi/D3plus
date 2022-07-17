using Catalog.Application.Common.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Services;

public class IntegrationEventService : IIntegrationEventService
{
    private readonly IBus _bus;
    private readonly ILogger<IntegrationEventService> _logger;

    public IntegrationEventService(IBus bus, ILogger<IntegrationEventService> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Publish<T>(T message, CancellationToken cancellationToken)
    {
        if (message == null)
            throw new ArgumentException("Message can't be null");

        try
        {
            await _bus.Publish(message, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message {MessageType}", typeof(T).Name);
            throw;
        }
    }
}
