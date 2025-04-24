namespace EcommercePortfolio.Domain.Payments;

public interface IPaymentApiService
{
    Task<bool> AuthorizePayment(Payment payment);
}
