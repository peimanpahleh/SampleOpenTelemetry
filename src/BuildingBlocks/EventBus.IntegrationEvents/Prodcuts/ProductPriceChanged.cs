namespace EventBus.IntegrationEvents.Prodcuts;

public record ProductPriceChanged(
    string ProductId,
    decimal OldPrice,
    decimal NewPrice) : IntegerationEvent;
