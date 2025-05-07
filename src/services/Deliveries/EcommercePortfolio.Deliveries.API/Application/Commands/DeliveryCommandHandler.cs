using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Core.Domain.ValueObjects;
using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Deliveries.Domain;
using EcommercePortfolio.Deliveries.Domain.ApiServices;
using EcommercePortfolio.Deliveries.Domain.Entities;
using MediatR;

namespace EcommercePortfolio.Deliveries.API.Application.Commands;

public class DeliveryCommandHandler(
    INotificationContext notification,
    IDeliveryRepository deliveryRepository,
    IOrderApiService orderApiService) : CommandHandler(notification),
    IRequestHandler<CreateDeliveryCommand>
{
    private readonly IDeliveryRepository _deliveryRepository = deliveryRepository;
    private readonly IOrderApiService _orderApiService = orderApiService;

    public async Task Handle(CreateDeliveryCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid())
        {
            AddError(message.ValidationResult);
            return;
        }

        var orderAddress = await _orderApiService.GetAddressOrderById(message.OrderId);
        if (orderAddress == null)
        {
            AddError("Order Address not found", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        var delivery = Delivery.CreateDelivery(message.OrderId, message.ClientId);
        delivery.SetAddress((Address)orderAddress);

        if (!IsDeliveredValid(delivery)) return;

        await _deliveryRepository.AddAsync(delivery);

        await _deliveryRepository.UnitOfWork.Commit();
    }

    private bool IsDeliveredValid(Delivery delivery)
    {
        var validationErrorMessage = delivery.ValidateForCreation();

        if (!string.IsNullOrWhiteSpace(validationErrorMessage))
        {
            AddError(validationErrorMessage, EnumNotificationType.VALIDATION_ERROR);
            return false;
        }

        return true;
    }
}
