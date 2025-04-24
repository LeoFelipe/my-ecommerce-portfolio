using EcommercePortfolio.Application.Orders.Events;
using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Core.Messaging.Mediator;
using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Domain.Carts;
using EcommercePortfolio.Domain.Carts.Entities;
using EcommercePortfolio.Domain.Orders;
using EcommercePortfolio.Domain.Orders.Entities;
using EcommercePortfolio.Domain.Payments;
using EcommercePortfolio.Domain.Payments.Enums;
using MediatR;

namespace EcommercePortfolio.Application.Orders.Commands;

public class OrderCommanderHandler(
    ICartRepository cartRepository,
    IOrderRepository orderRepository,
    IPaymentService paymentService,
    INotificationContext notification) : CommandHandler(notification),
    IRequestHandler<AddOrderCommand>
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IPaymentService _paymentService = paymentService;

    public async Task Handle(AddOrderCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid())
        {
            AddError(message.ValidationResult);
            return;
        }

        var cart = await _cartRepository.GetByIdAndClientId(message.CartId, message.ClientId);

        if (cart == null)
        {
            AddError("Cart not found", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        var order = MapToOrder(cart, message);

        if (!IsOrderValid(order)) return;

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

        order.AddEvent(new OrderAuthorizedEvent(message.CartId, order.Id, order.ClientId));

        await _orderRepository.AddAsync(order);

        await PersistData(_orderRepository.UnitOfWork);
    }

    private static Order MapToOrder(Cart cart, AddOrderCommand message)
    {
        var order = new Order(
            cart.ClientId,
            [.. cart.CartItems.Select(x => new OrderItem(x.ProductId, x.ProductName, x.Category, x.Quantity, x.Price))]);

        order.SetAddress(new Address
        {
            ZipCode = message.Address.ZipCode,
            State = message.Address.State,
            City = message.Address.City,
            StreetAddress = message.Address.StreetAddress,
            NumberAddres = message.Address.NumberAddress
        });

        return order;
    }

    private bool IsOrderValid(Order order)
    {
        var totalValue = order.TotalValue;
        
        order.CalculateTotalOrderValue();

        if (order.TotalValue != totalValue)
        {
            AddError("The order total value order is different from total value of individual items", EnumNotificationType.VALIDATION_ERROR);
            return false;
        }

        return true;
    }
}
