using EcommercePortfolio.Core.MessageBus;
using EcommercePortfolio.Core.Messaging.Integrations;
using MediatR;

namespace EcommercePortfolio.Application.Orders.Events;

public class OrderEventHandler(IMessageBus messageBus) : 
    INotificationHandler<OrderAuthorizedEvent>
{
    private readonly IMessageBus _messageBus = messageBus;

    public async Task Handle(OrderAuthorizedEvent message, CancellationToken cancellationToken)
    {
        await _messageBus.Publish(new UpdateCartForOrderAuthorizedIntegrationMessage(message.ClientId));


    }
}   
