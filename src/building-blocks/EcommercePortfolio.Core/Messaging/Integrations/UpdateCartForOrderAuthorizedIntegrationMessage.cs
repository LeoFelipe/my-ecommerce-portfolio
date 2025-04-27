namespace EcommercePortfolio.Core.Messaging.Integrations;

public record UpdateCartForOrderAuthorizedIntegrationMessage(Guid ClientId) : IntegrationMessage;