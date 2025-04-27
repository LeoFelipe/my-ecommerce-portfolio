using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Core.ExternalServices.MyFakePay;
using EcommercePortfolio.Core.ExternalServices.MyFakePay.Enums;
using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Domain.ValueObjects;
using EcommercePortfolio.Orders.API.Application.Events;
using EcommercePortfolio.Orders.Domain;
using EcommercePortfolio.Orders.Domain.ApiServices;
using EcommercePortfolio.Orders.Domain.Entities;
using MediatR;

namespace EcommercePortfolio.Orders.API.Application.Commands;

public class OrderCommanderHandler(
    ICartApiService cartApiService,
    IOrderRepository orderRepository,
    IPaymentService paymentService,
    INotificationContext notification) : CommandHandler(notification),
    IRequestHandler<CreateOrderCommand>
{
    private readonly ICartApiService _cartApiService = cartApiService;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IPaymentService _paymentService = paymentService;

    public async Task Handle(CreateOrderCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid())
        {
            AddError(message.ValidationResult);
            return;
        }

        var cart = await _cartApiService.GetCartByClientId(message.ClientId);
        if (cart == null || string.IsNullOrWhiteSpace(cart.Id))
        {
            AddError("Cart not found", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        var order = MapToOrder(cart, message);
        
        var paymentDone = await _paymentService.DoPayment(new Payment(
            order.Id,
            order.ClientId,
            order.TotalValue,
            message.PaymentMethod));

        if (paymentDone.PaymentStatus != EnumPaymentStatus.AUTHORIZED)
        {
            AddError("Payment not authorized", EnumNotificationType.VALIDATION_ERROR);
            return;
        }

        order.PaymentAuthorized(paymentDone.Id);

        order.AddEvent(new OrderAuthorizedEvent(order.Id, order.ClientId));


        if (!IsOrderValid(order, cart)) return;

        await _orderRepository.AddAsync(order);

        await PersistData(_orderRepository.UnitOfWork);
    }

    private static Order MapToOrder(GetCartByClientIdResponse cart, CreateOrderCommand message)
    {
        var order = Order.CreateOrder(
            cart.ClientId,
            [.. cart.CartItems.Select(x => OrderItem.CreateOrderItem(x.ProductId, x.Name, x.Category, x.Quantity, x.Price))]);

        order.SetAddress(Address.CreateAddress(
            message.Address.ZipCode,
            message.Address.State,
            message.Address.City,
            message.Address.StreetAddress,
            message.Address.NumberAddress));

        return order;
    }

    private bool IsOrderValid(Order order, GetCartByClientIdResponse cart)
    {
        if (order.AnyEventType(typeof(OrderAuthorizedEvent)))
            throw new DomainException("Event OrderAuthorizedEvent not found");

        var validationErrorMessage = order.ValidateForCreation(cart.TotalValue);

        if (!string.IsNullOrWhiteSpace(validationErrorMessage))
        {
            AddError(validationErrorMessage, EnumNotificationType.VALIDATION_ERROR);
            return false;
        }

        return true;
    }
}
