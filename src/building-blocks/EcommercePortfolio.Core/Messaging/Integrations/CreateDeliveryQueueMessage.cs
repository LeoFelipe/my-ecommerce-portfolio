namespace EcommercePortfolio.Core.Messaging.Integrations;

public record CreateDeliveryQueueMessage(
    Guid OrderId,
    Guid ClientId) : Command;