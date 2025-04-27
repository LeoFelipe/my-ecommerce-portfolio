namespace EcommercePortfolio.Core.ExternalServices.MyFakePay;

public interface IPaymentService
{
    Task<Payment> DoPayment(Payment payment);
}
