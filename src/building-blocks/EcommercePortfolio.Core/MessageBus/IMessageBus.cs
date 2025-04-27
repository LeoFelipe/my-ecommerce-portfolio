using EcommercePortfolio.Core.Messaging;

namespace EcommercePortfolio.Core.MessageBus;

public interface IMessageBus
{
    Task Publish<T>(T message) where T : IntegrationMessage;
}
