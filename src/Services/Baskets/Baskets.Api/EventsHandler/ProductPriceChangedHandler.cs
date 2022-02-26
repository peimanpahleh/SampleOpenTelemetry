namespace Baskets.Api.EventsHandler;

public class ProductPriceChangedHandler : IIntegrationEventHandler<ProductPriceChanged>
{
    public Task Consume(ConsumeContext<ProductPriceChanged> context)
    {
        return Task.CompletedTask;
    }
}
