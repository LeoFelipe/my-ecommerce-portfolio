using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Domain.Payments.Enums;

namespace EcommercePortfolio.Domain.Payments;

public class Payment : SqlEntity, IAggregateRoot
{
    public Guid OrderId { get; private set; }
    public Guid ClientId { get; private set; }
    public decimal PaymentTotalValue { get; private set; }
    public DateTime PaymentDate { get; }
    public EnumPaymentMethod PaymentMethod { get; private set; }
    public EnumPaymentStatus PaymentStatus { get; private set; }

    public Payment(Guid orderId, Guid clientId, decimal paymentTotalValue, EnumPaymentMethod paymentMethod)
    {
        OrderId = orderId;
        ClientId = clientId;
        PaymentTotalValue = paymentTotalValue;
        PaymentMethod = paymentMethod;
        PaymentDate = DateTime.Now;
    }

    protected Payment() { }

    public void AuthorizePayment() => PaymentStatus = EnumPaymentStatus.AUTHORIZED;
    public void PayPayment() => PaymentStatus = EnumPaymentStatus.PAID;
    public void DenyPayment() => PaymentStatus = EnumPaymentStatus.DENIED;
    public void RefundPayment() => PaymentStatus = EnumPaymentStatus.REFUND;
    public void CancelPayment() => PaymentStatus = EnumPaymentStatus.CANCELED;
}
