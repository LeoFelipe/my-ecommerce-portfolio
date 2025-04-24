using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Domain.Orders.Enums;

namespace EcommercePortfolio.Domain.Orders.Entities;

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


    public Order(Guid clientId, List<OrderItem> orderItems)
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
        TotalValue = amount < 0 ? 0 : amount;
    }
}