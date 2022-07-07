namespace Carting.WebApi.Infrastructure.Consumers;

using Carting.WebApi.Application.Common.Interfaces;
using Carting.WebApi.Domain.Entities;
using MassTransit;
using Messaging.Contracts;

class ProductPriceChangedIntegrationEventConsumer :
    IConsumer<ProductPriceChangedIntegrationEvent>
{
    private readonly ICartingDbContext _context;
    private readonly ILogger<ProductPriceChangedIntegrationEventConsumer> _logger;

    public ProductPriceChangedIntegrationEventConsumer(ICartingDbContext context, ILogger<ProductPriceChangedIntegrationEventConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
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
            _logger.LogError("Updating the price of {ProductId} has been failed. Exception: {Exception}", context.Message.ProductId, ex.Message);
            throw;
        }
    }
}


class ProductPriceChangedIntegrationEventConsumerDefinition :
    ConsumerDefinition<ProductPriceChangedIntegrationEventConsumer>
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
