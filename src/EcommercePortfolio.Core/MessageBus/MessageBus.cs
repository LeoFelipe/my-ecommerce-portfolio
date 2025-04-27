using EcommercePortfolio.Core.Messaging;
using MassTransit;

namespace EcommercePortfolio.Core.MessageBus;

public class MessageBus(IBus messageBus) : IMessageBus
{
    private readonly IBus _messageBus = messageBus;

    public async Task Publish<T>(T message) where T : IntegrationMessage
    {
        await _messageBus.Publish(message);
    }
}
