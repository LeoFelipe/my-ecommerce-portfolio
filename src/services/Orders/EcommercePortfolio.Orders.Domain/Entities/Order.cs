using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Domain.ValueObjects;
using EcommercePortfolio.Orders.Domain.Enums;

namespace EcommercePortfolio.Orders.Domain.Entities;

public class Order : SqlEntity, IAggregateRoot
{
    public Guid PaymentId { get; private set; }
    public Guid ClientId { get; private set; }
    public DateTime CreatedAt { get; }
    public decimal TotalValue { get; private set; }
    public EnumOrderStatus OrderStatus { get; private set; }
    public virtual Address Address { get; private set; }


    private readonly List<OrderItem> _orderItems;
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;


    private Order(Guid clientId, List<OrderItem> orderItems)
    {
        ClientId = clientId;
        _orderItems = orderItems;

        CalculateTotalOrderValue();
    }

    protected Order() { }

    public void Authorize() => OrderStatus = EnumOrderStatus.AUTHORIZED;
    public void Cancel() => OrderStatus = EnumOrderStatus.CANCELED;
    public void Approve() => OrderStatus = EnumOrderStatus.APPROVED;

    public void SetPayment(Guid paymentId) => PaymentId = paymentId;
    public void SetAddress(Address address) => Address = address;

    public void PaymentAuthorized(Guid paymentId)
    {
        SetPayment(paymentId);
        Authorize();
    }

    public void CalculateTotalOrderValue()
    {
        var amount = OrderItems.Sum(x => x.CalculateTotalAmount());
        TotalValue = amount <= 0 
            ? throw new DomainException("Order total value is 0")
            : amount;
    }

    public static Order CreateOrder(Guid clientId, List<OrderItem> orderItems)
    {
        if (clientId == Guid.Empty) throw new DomainException("Client id not informed");
        if (orderItems == null || orderItems.Count == 0) throw new DomainException("Order items not informed");
        if (orderItems.Sum(x => x.Quantity) <= 0) throw new DomainException("Order items quantity not valid");
        if (orderItems.Sum(x => x.Price) <= 0) throw new DomainException("Order items price not valid");

        return new Order(clientId, orderItems);
    }

    public string ValidateForCreation(decimal cartTotalValue)
    {
        if (TotalValue != cartTotalValue)
            return "The order total value is different from cart total value";

        if (OrderStatus != EnumOrderStatus.AUTHORIZED)
            return "The order is not authorized";

        return string.Empty;
    }
}