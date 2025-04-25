namespace EcommercePortfolio.Core.Messaging.Integration;

public record OrderAuthorizedIntegrationEvent(Guid ClientId): IntegrationEvent;