using System.Reflection;
using Catalog.Application.Common.Interfaces;
using Messaging.Contracts;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json;

namespace Catalog.WebApi.HostedServices;

public class OutboxHostedService : IHostedService, IDisposable
{
    private readonly ILogger<OutboxHostedService> _logger;
    private Timer? _timer = null;
    private readonly IServiceProvider _serviceProvider;
    private readonly TelemetryConfiguration _telemetryConfiguration;
    private readonly IConfiguration _configuration;
    public OutboxHostedService(ILogger<OutboxHostedService> logger, IServiceProvider serviceProvider, IConfiguration configuration
        )
    {
        _configuration = configuration;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _telemetryConfiguration = TelemetryConfiguration.CreateDefault();
        _telemetryConfiguration.ConnectionString = configuration["ApplicationInsights:ConnectionString"];
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
        var telemetryClient = new TelemetryClient(_telemetryConfiguration);

        using (var operation = telemetryClient.StartOperation<DependencyTelemetry>("OutboxService"))
        {
            operation.Telemetry.Type = "Background";
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
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

                operation.Telemetry.Success = true;
                telemetryClient.TrackTrace($"Scheduled process trace");

            }
            catch (Exception ex)
            {
                _logger.LogError("Error while processing messages {Message}", ex.Message);
                operation.Telemetry.Success = false;
                telemetryClient.TrackException(ex);
            }
            finally
            {
                telemetryClient.StopOperation(operation);
            }
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
