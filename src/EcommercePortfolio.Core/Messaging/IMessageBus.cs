namespace EcommercePortfolio.Core.Messaging;

public interface IMessageBus
{
    Task Publish<T>(T message) where T : Message;
}
