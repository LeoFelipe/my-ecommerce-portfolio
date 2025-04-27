
namespace EcommercePortfolio.Core.ExternalServices.MyFakePay;

public class PaymentService(
    IPaymentApiService paymentApiService) : IPaymentService
{
    private readonly IPaymentApiService _paymentApiService = paymentApiService;

    public async Task<Payment> DoPayment(Payment payment)
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
