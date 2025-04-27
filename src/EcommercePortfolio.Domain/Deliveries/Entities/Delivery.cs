using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Domain.Deliveries.Enums;
using EcommercePortfolio.Domain.Deliveries.ValueObjects;

namespace EcommercePortfolio.Domain.Deliveries.Entities;

public class Delivery : SqlEntity, IAggregateRoot
{
    public Guid OrderId { get; private set; }
    public Guid ClientId { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime ExpectedDate { get; private set; }
    public DateTime? DateMade { get; private set; }
    public EnumDeliveryStatus DeliveryStatus { get; private set; }
    public virtual Address Address { get; private set; }

    public Delivery(Guid orderId, Guid clientId, DateTime expectedDate)
    {
        OrderId = orderId;
        ClientId = clientId;
        ExpectedDate = expectedDate;
        DeliveryStatus = EnumDeliveryStatus.PENDING;
    }

    protected Delivery() { }

    public void SetExpectedDate(DateTime expectedDate) => ExpectedDate = expectedDate;
    public void SetDateMade(DateTime dateMade) => DateMade = dateMade;
    public void SetAddress(Address address) => Address = address;
}
