namespace Carting.WebApi.Infrastructure.Consumers;

using Carting.WebApi.Application.Common.Interfaces;
using Carting.WebApi.Domain.Entities;
using MassTransit;
using Messaging.Contracts;

class ProductDeletedIntegrationEventConsumer :
    IConsumer<ProductDeletedIntegrationEvent>
{
    private readonly ICartingDbContext _context;
    private readonly ILogger<ProductDeletedIntegrationEventConsumer> _logger;

    public ProductDeletedIntegrationEventConsumer(ICartingDbContext context, ILogger<ProductDeletedIntegrationEventConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductDeletedIntegrationEvent> context)
    {
        try
        {
            var carts = _context.GetAll<Cart>(c => c.Items.Any(i => i.Id == context.Message.ProductId)).ToList();

            foreach (var cart in carts)
            {
                var cartItem = cart.Items.FirstOrDefault(i => i.Id == context.Message.ProductId);
                if (cartItem == null)
                    break;

                cart.Items.Remove(cartItem);
                _context.Update(cart);
                _logger.LogInformation("Product {ProductId} has removed from {CartId}", context.Message.ProductId, cart.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Deleting item {ProductId} has been failed. Exception: {Exception}", context.Message.ProductId, ex.Message);
            throw;
        }
    }
}


class ProductDeletedIntegrationEventConsumerDefinition :
    ConsumerDefinition<ProductDeletedIntegrationEventConsumer>
{
    public ProductDeletedIntegrationEventConsumerDefinition()
    {
        // override the default endpoint name
        EndpointName = "carting-service";
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ProductDeletedIntegrationEventConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
