using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Core.Messaging.Integrations;
using EcommercePortfolio.Core.Notification;
using MediatR;

namespace EcommercePortfolio.Deliveries.API.Application.Commands;

public class DeliveryCommandHandler(
    INotificationContext notification) : CommandHandler(notification),
    IRequestHandler<CreateDeliveryQueueMessage>
{
    public async Task Handle(CreateDeliveryQueueMessage message, CancellationToken cancellationToken)
    {
        if (!message.IsValid())
        {
            AddError(message.ValidationResult);
            return;
        }

        await Task.CompletedTask;
    }
}
