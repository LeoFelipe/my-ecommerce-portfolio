namespace EcommercePortfolio.Core.Messaging.Integrations;

public record OrderAuthorizedQueueMessage(
    Guid OrderId,
    Guid ClientId) : IntegrationEvent;