using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Core.Notification;
using MediatR;

namespace EcommercePortfolio.Deliveries.API.Application.Commands;

public class DeliveryCommanderHandler(
    INotificationContext notification) : CommandHandler(notification),
    IRequestHandler<CreateDeliveryCommand>
{
    public async Task Handle(CreateDeliveryCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid())
        {
            AddError(message.ValidationResult);
            return;
        }

        await Task.CompletedTask;
    }
}
