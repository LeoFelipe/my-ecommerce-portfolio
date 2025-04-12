using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Domain.Orders.Enums;

namespace EcommercePortfolio.Domain.Orders.Entities;

public class Order : Entity, IAggregateRoot
{
    public int? PaymentId { get; }
    public Guid ClientId { get; private set; }
    public DateTime OrderDate { get; }
    public decimal TotalValue { get; private set; }
    public EnumOrderStatus OrderStatus { get; private set; }

    public virtual Address Address { get; private set; }


    private readonly List<OrderItem> _orderItems;
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();


    public Order(Guid clientId, List<OrderItem> orderItems)
    {
        ClientId = clientId;
        _orderItems = orderItems;

        CalculateTotalOrderValue(orderItems);
    }

    protected Order() { }

    public void Authorize() => OrderStatus = EnumOrderStatus.AUTHORIZED;
    public void Cancel() => OrderStatus = EnumOrderStatus.CANCELED;
    public void Approve() => OrderStatus = EnumOrderStatus.APPROVED;

    public void SetAddress(Address address) => Address = address;

    private void CalculateTotalOrderValue(List<OrderItem> orderItems)
    {
        var amount = orderItems.Sum(x => x.CalculateTotalAmount());
        TotalValue = amount < 0 ? 0 : amount;
    }
}