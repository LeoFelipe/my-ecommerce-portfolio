using EcommercePortfolio.Core.Messaging;

namespace EcommercePortfolio.Core.Mediator;

public interface IMediatorHandler
{
    Task SendCommand<T>(T message) where T : Command;
    Task PublishEvent<T>(T message) where T : Event;
}
