namespace EcommercePortfolio.ExternalServices.MyFakePayApi;

public interface IPaymentService
{
    Task<PaymentApiResponse> DoPayment(PaymentApiResponse payment);
}
