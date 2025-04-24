using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Core.Messaging.Integration;
using MediatR;

namespace EcommercePortfolio.Application.Orders.Events;

public class OrderEventHandler(IMessageBus messageBus) : 
    INotificationHandler<OrderAuthorizedEvent>
{
    private readonly IMessageBus _messageBus = messageBus;

    public async Task Handle(OrderAuthorizedEvent message, CancellationToken cancellationToken)
    {
        await _messageBus.Publish(new OrderAuthorizedIntegrationEvent(message.CartId, message.ClientId));
    }
}   
