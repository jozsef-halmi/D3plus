using System.Reflection;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Outbox;
using Messaging.Contracts;

namespace Catalog.WebApi.HostedServices;

public class OutboxHostedService : IHostedService, IDisposable
{
    private readonly ILogger<OutboxHostedService> _logger;
    private Timer? _timer = null;
    private readonly IOutboxService _outboxService;
    private readonly IServiceProvider _serviceProvider;

    public OutboxHostedService(
        ILogger<OutboxHostedService> logger,
        //, IOutboxService outboxService,
        IServiceProvider serviceProvider
        )
    {
        _logger = logger;
        //_outboxService = outboxService;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbux Hosted Service running.");

        _timer = new Timer(async o => await ProcessMessages(o), null, TimeSpan.Zero,
            TimeSpan.FromSeconds(15));

        return Task.CompletedTask;
    }

    //private void DoWork(object? state)
    //{
    //    using var scope = _serviceProvider.CreateScope();
    //    var outboxService = new OutboxService
    //    CancellationTokenSource cts = new CancellationTokenSource();
    //    _outboxService.ProcessMessages(cts.Token);
    //}

    public async Task ProcessMessages(object? state)
    {
        try
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var integrationEventService = scope.ServiceProvider.GetRequiredService<IIntegrationEventService>();
            var messagesToBeProcessed = dbContext.OutboxMessages.Where(m => m.PublishedDate == null).ToList();

            foreach (var message in messagesToBeProcessed)
            {
                Type integrationEventType = Assembly.GetAssembly(typeof(IntegrationEvent)).GetType(message.IntegrationEventType);
                
                var integrationEvent = System.Text.Json.JsonSerializer.Deserialize(message.IntegrationEventJson, integrationEventType);

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
