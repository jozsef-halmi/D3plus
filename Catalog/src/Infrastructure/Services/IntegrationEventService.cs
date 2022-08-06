using Catalog.Application.Common.Interfaces;
using MassTransit;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Services;

public class IntegrationEventService : IIntegrationEventService
{
    private readonly IBus _bus;
    private readonly ILogger<IntegrationEventService> _logger;
    private readonly TelemetryConfiguration _telemetryConfiguration;

    public IntegrationEventService(IBus bus, ILogger<IntegrationEventService> logger, IConfiguration configuration)
    {
        _bus = bus;
        _logger = logger;
        _telemetryConfiguration = TelemetryConfiguration.CreateDefault();
        _telemetryConfiguration.ConnectionString = configuration["ApplicationInsights:ConnectionString"];
    }

    public async Task Publish<T>(T message, CancellationToken cancellationToken)
    {
        if (message == null)
            throw new ArgumentException("Message can't be null");

        var telemetryClient = new TelemetryClient(_telemetryConfiguration)
        {
        };

        using (var operation = telemetryClient.StartOperation<DependencyTelemetry>("RabbitMQ"))
        {
            try
            {
                await _bus.Publish(message, cancellationToken);
                operation.Telemetry.Success = true;
                telemetryClient.TrackTrace($"Scheduled process trace");
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                _logger.LogError(ex, "Error sending message {MessageType}", typeof(T).Name);
                throw;
            }
            finally
            {
                telemetryClient.StopOperation(operation);
            }
        }
          
    }
}
