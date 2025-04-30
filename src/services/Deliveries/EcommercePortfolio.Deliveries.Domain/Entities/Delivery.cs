using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Deliveries.Domain.Enums;
using EcommercePortfolio.Domain.ValueObjects;

namespace EcommercePortfolio.Deliveries.Domain.Entities;

public class Delivery : SqlEntity, IAggregateRoot
{
    public Guid OrderId { get; private set; }
    public Guid ClientId { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime ExpectedDate { get; private set; }
    public DateTime? DateMade { get; private set; }
    public EnumDeliveryStatus DeliveryStatus { get; private set; }
    public virtual Address Address { get; private set; }

    private Delivery(Guid orderId, Guid clientId, DateTime expectedDate)
    {
        OrderId = orderId;
        ClientId = clientId;
        ExpectedDate = expectedDate;
        DeliveryStatus = EnumDeliveryStatus.PENDING;
    }

    protected Delivery() { }

    public static Delivery CreateDelivery(Guid orderId, Guid clientId)
    {
        return new Delivery(orderId, clientId, DateTime.Now.AddDays(30));
    }

    public void SetDateMade(DateTime dateMade) => DateMade = dateMade;
    public void SetAddress(Address address) => Address = address;
}
