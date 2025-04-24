using MassTransit;

namespace EcommercePortfolio.Core.Messaging;

public class MessageBus(IBus messageBus) : IMessageBus
{
    private readonly IBus _messageBus = messageBus;

    public async Task Publish<T>(T message) where T : Message
    {
        await _messageBus.Publish(message);
    }
}
