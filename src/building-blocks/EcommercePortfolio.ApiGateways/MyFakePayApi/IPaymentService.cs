namespace EcommercePortfolio.ApiGateways.MyFakePayApi;

public interface IPaymentService
{
    Task<PaymentApiResponse> DoPayment(PaymentApiResponse payment);
}
