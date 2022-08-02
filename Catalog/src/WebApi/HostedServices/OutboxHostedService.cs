using Catalog.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace Catalog.WebApi.HostedServices;

public class OutboxHostedService : IHostedService, IDisposable
{
    private readonly ILogger<OutboxHostedService> _logger;
    private Timer? _timer = null;
    private readonly IServiceProvider _serviceProvider;

    public OutboxHostedService(ILogger<OutboxHostedService> logger, IServiceProvider serviceProvider
        )
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Hosted Service running.");

        _timer = new Timer(async o => await ProcessMessages(o), null, TimeSpan.Zero,
            TimeSpan.FromSeconds(15));

        return Task.CompletedTask;
    }


    public async Task ProcessMessages(object? state)
    {
        try
        {
            CancellationTokenSource cts = new();
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var integrationEventService = scope.ServiceProvider.GetRequiredService<IIntegrationEventService>();
            var messagesToBeProcessed = dbContext.OutboxMessages.Where(m => m.PublishedDate == null).ToList();

            foreach (var message in messagesToBeProcessed)
            {
                var integrationEvent = JsonConvert.DeserializeObject(message.IntegrationEventJson, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                await integrationEventService.Publish(integrationEvent, cts.Token);
                message.PublishedDate = DateTime.UtcNow;
                await dbContext.SaveChangesAsync(cts.Token);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while processing messages {Message}", ex.Message);
        }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
