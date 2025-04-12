using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Domain.Payment.Enums;

namespace EcommercePortfolio.Domain.Payment;

public class Payment : Entity, IAggregateRoot
{
    public Guid OrderId { get; private set; }
    public decimal PaymentTotalValue { get; private set; }
    public DateTime PaymentDate { get; }
    public EnumPaymentMethod PaymentMethod { get; private set; }
    public EnumPaymentStatus PaymentStatus { get; private set; }

    public Payment(Guid orderId, decimal paymentTotalValue, EnumPaymentMethod paymentMethod)
    {
        OrderId = orderId;
        PaymentTotalValue = paymentTotalValue;
        PaymentMethod = paymentMethod;
    }

    protected Payment() { }

    public void AuthorizePayment() => PaymentStatus = EnumPaymentStatus.AUTHORIZED;
    public void PayPayment() => PaymentStatus = EnumPaymentStatus.PAID;
    public void DenyPayment() => PaymentStatus = EnumPaymentStatus.DENIED;
    public void RefundPayment() => PaymentStatus = EnumPaymentStatus.REFUND;
    public void CancelPayment() => PaymentStatus = EnumPaymentStatus.CANCELED;
}
