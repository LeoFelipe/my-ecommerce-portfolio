using EcommercePortfolio.Core.MessageBus;
using EcommercePortfolio.Core.Messaging.Integrations;
using MediatR;

namespace EcommercePortfolio.Orders.API.Application.Events;

public class OrderEventHandler(
    IMessageBus messageBus) : 
    INotificationHandler<OrderAuthorizedEvent>
{
    private readonly IMessageBus _messageBus = messageBus;

    public async Task Handle(OrderAuthorizedEvent message, CancellationToken cancellationToken)
    {
        await _messageBus.Publish(new OrderAuthorizedQueueMessage(message.Id, message.ClientId));
    }
}   
