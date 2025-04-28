using EcommercePortfolio.Core.Messaging;

namespace EcommercePortfolio.Core.Messaging.Integrations;

public record RemoveCartQueueMessage(Guid ClientId) : IntegrationEvent;