using EcommercePortfolio.Core.MessageBus;
using EcommercePortfolio.Core.MessageBus.QueueMessages;
using EcommercePortfolio.Core.Messaging.Integrations.Commands;
using MediatR;

namespace EcommercePortfolio.Orders.API.Application.Events;

public class OrderEventHandler(
    IMessageBus messageBus,
    IMediator mediator) : 
    INotificationHandler<OrderAuthorizedEvent>
{
    private readonly IMessageBus _messageBus = messageBus;
    private readonly IMediator _mediator = mediator;

    public async Task Handle(OrderAuthorizedEvent message, CancellationToken cancellationToken)
    {
        var taskPublishOrderAuthorized = 
            _messageBus.Publish(new RemoveCartQueueMessage(message.ClientId));

        var taskSendCreateDelivery = 
            _mediator.Send(new CreateDeliveryIntegrationCommand(message.Id, message.ClientId), cancellationToken);

        await Task.WhenAll(taskPublishOrderAuthorized, taskSendCreateDelivery);
    }
}   
