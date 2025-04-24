namespace EcommercePortfolio.Domain.Payments;

public interface IPaymentService
{
    Task<Payment> DoPayment(Payment payment);
}
