using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Deliveries.Domain;
using EcommercePortfolio.Deliveries.Domain.Entities;
using MediatR;

namespace EcommercePortfolio.Deliveries.API.Applications.Commands;

public class DeliveryCommandHandler(
    INotificationContext notification,
    IDeliveryRepository deliveryRepository) : CommandHandler(notification),
    IRequestHandler<CreateDeliveryCommand>
{
    private readonly IDeliveryRepository _deliveryRepository = deliveryRepository;

    public async Task Handle(CreateDeliveryCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid())
        {
            AddError(message.ValidationResult);
            return;
        }

        var delivery = Delivery.CreateDelivery(message.OrderId, message.ClientId);

        await _deliveryRepository.AddAsync(delivery);

        await _deliveryRepository.UnitOfWork.Commit();
    }
}
