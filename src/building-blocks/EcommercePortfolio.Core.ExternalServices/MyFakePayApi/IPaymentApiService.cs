namespace EcommercePortfolio.ExternalServices.MyFakePayApi;

public interface IPaymentApiService
{
    Task<bool> AuthorizePayment(PaymentApiResponse payment);
}
