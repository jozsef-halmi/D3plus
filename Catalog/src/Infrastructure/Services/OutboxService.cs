using Catalog.Application.Common.Interfaces;
using Catalog.Application.Outbox;
using Messaging.Contracts;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Services;

public class OutboxService : IOutboxService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<OutboxService> _logger;
    private readonly IIntegrationEventService _integrationEventService;

    public OutboxService(IApplicationDbContext dbContext, ILogger<OutboxService> logger, IIntegrationEventService integrationEventService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _integrationEventService = integrationEventService;
    }

    public async Task ProcessMessages(CancellationToken cancellationToken)
    {
        try
        {
            var messagesToBeProcessed = _dbContext.OutboxMessages.Where(m => m.PublishedDate == null);

            foreach (var message in messagesToBeProcessed)
            {
                var integrationEvent = System.Text.Json.JsonSerializer.Deserialize(message.IntegrationEventJson, Type.GetType(message.IntegrationEventType));
                await _integrationEventService.Publish(integrationEvent, cancellationToken);
                message.PublishedDate = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while processing messages {Message}", ex.Message);
        }
    }
}
