namespace Carting.WebApi.Infrastructure.Consumers;

using System.Diagnostics;
using Carting.WebApi.Application.Common.Interfaces;
using Carting.WebApi.Domain.Entities;
using MassTransit;
using Messaging.Contracts;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

internal sealed class ProductPriceChangedIntegrationEventConsumer :
    IConsumer<ProductPriceChangedIntegrationEvent>
{
    private readonly ICartingDbContext _context;
    private readonly ILogger<ProductPriceChangedIntegrationEventConsumer> _logger;
    private readonly TelemetryConfiguration _telemetryConfiguration;

    public ProductPriceChangedIntegrationEventConsumer(ICartingDbContext context, ILogger<ProductPriceChangedIntegrationEventConsumer> logger, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _telemetryConfiguration = TelemetryConfiguration.CreateDefault();
        _telemetryConfiguration.ConnectionString = configuration["ApplicationInsights:ConnectionString"];
    }

    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        var name = "Consume "+context.Message.GetType().Name;
        RequestTelemetry requestTelemetry = new RequestTelemetry { Name = name };
        var telemetryClient = new TelemetryClient(_telemetryConfiguration);

        requestTelemetry.Context.Operation.Id = context.Message.TraceRootId;
        requestTelemetry.Context.Operation.ParentId = context.Message.TraceParentId;

        using var operation = telemetryClient.StartOperation(requestTelemetry);

        try
        {
            if (context.Message.OldPrice == context.Message.NewPrice)
            {
                _logger.LogWarning("Got ProductPriceChangedIntegrationEvent with the same old and new price values {OldPrice} {NewPrice}, {ProductId}", context.Message.OldPrice, context.Message.NewPrice, context.Message.ProductId);
                return;
            }

            var carts = _context.GetAll<Cart>(c => c.Items.Any(i => i.Id == context.Message.ProductId)).ToList();

            foreach (var cart in carts)
            {
                var cartItem = cart.Items.First(i => i.Id == context.Message.ProductId);
                cartItem.Price = context.Message.NewPrice;
                _context.Update(cart);
                _logger.LogInformation("Product price of {ProductId} in {CartId} has been updated from {OldPrice} to {NewPrice}", context.Message.ProductId, cart.Id, context.Message.OldPrice, context.Message.NewPrice);
            }
        }
        catch (Exception ex)
        {
            telemetryClient.TrackException(ex);
            _logger.LogError(ex, "Updating the price of {ProductId} has been failed.", context.Message.ProductId);
            throw;
        }
        finally
        {
            telemetryClient.StopOperation(operation);
        }
    }
}


internal sealed class ProductPriceChangedIntegrationEventConsumerDefinition : ConsumerDefinition<ProductPriceChangedIntegrationEventConsumer>
{
    public ProductPriceChangedIntegrationEventConsumerDefinition()
    {
        // override the default endpoint name
        EndpointName = "carting-service";
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ProductPriceChangedIntegrationEventConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
    }
}
