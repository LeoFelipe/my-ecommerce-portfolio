namespace EcommercePortfolio.Core.Messaging.Integration;

public record OrderAuthorizedIntegrationEvent(
    string CartId,
    Guid ClientId): IntegrationEvent;