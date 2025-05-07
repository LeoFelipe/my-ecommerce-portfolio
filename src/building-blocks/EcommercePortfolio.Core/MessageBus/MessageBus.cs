using EcommercePortfolio.Core.Messaging;
using MassTransit;

namespace EcommercePortfolio.Core.MessageBus;

public class MessageBus(IBus bus) : IMessageBus
{
    private readonly IBus _bus = bus;

    public async Task Publish<T>(T message) where T : IntegrationEvent
    {
        await _bus.Publish(message);
    }
}
