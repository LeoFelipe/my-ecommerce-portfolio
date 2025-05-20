namespace EcommercePortfolio.ApiGateways.MyFakePayApi;

public interface IPaymentApiService
{
    Task<bool> AuthorizePayment(PaymentApiResponse payment);
}
