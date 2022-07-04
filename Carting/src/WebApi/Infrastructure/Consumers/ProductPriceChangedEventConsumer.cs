namespace Carting.WebApi.Infrastructure.Consumers;

using MassTransit;
using Messaging.Contracts;

class ProductPriceChangedIntegrationEventConsumer :
    IConsumer<ProductPriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        var a = 5;
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

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
