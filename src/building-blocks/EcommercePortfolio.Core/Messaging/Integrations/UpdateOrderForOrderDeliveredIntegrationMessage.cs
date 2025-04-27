namespace EcommercePortfolio.Core.Messaging.Integrations;

public record UpdateOrderForOrderDeliveredIntegrationMessage(Guid OrderId) : IntegrationMessage;