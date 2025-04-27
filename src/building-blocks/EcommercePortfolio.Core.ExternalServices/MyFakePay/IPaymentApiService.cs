namespace EcommercePortfolio.Core.ExternalServices.MyFakePay;

public interface IPaymentApiService
{
    Task<bool> AuthorizePayment(Payment payment);
}
