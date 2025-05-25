namespace EcommercePortfolio.ApiGateways.MyFakePaymentApi;

public interface IPaymentService
{
    Task<PaymentApiResponse> DoPayment(PaymentApiResponse payment);
}
