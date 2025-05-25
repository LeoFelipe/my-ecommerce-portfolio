namespace EcommercePortfolio.ApiGateways.MyFakePaymentApi;

public interface IPaymentApiService
{
    Task<bool> AuthorizePayment(PaymentApiResponse payment);
}
