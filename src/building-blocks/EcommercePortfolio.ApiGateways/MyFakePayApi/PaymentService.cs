namespace EcommercePortfolio.ApiGateways.MyFakePayApi;

public class PaymentService(
    IPaymentApiService paymentApiService) : IPaymentService
{
    private readonly IPaymentApiService _paymentApiService = paymentApiService;

    public async Task<PaymentApiResponse> DoPayment(PaymentApiResponse payment)
    {
        var authorized = await _paymentApiService.AuthorizePayment(payment);

        if (authorized)
        {
            payment.AuthorizePayment();
            return payment;
        }

        payment.DenyPayment();
        return payment;
    }
}
