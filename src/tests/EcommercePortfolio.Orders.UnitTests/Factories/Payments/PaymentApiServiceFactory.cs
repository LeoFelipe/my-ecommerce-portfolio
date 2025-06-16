using Bogus;
using EcommercePortfolio.ApiGateways.MyFakePay.Enums;
using EcommercePortfolio.ApiGateways.MyFakePaymentApi;

namespace EcommercePortfolio.Orders.UnitTests.Factories.Payments;

public static class PaymentApiServiceFactory
{
    public static PaymentApiResponse BuildValidPayment(Guid clientId, decimal paymentTotalValue)
    {
        var payment = new PaymentApiResponse(
             Guid.CreateVersion7(),
             clientId,
             paymentTotalValue,
             EnumPaymentMethod.PIX);

        payment.AuthorizePayment();

        return payment;
    }

    public static PaymentApiResponse BuildPaymentNotAuthorized(Guid clientId)
    {
        var payment = new PaymentApiResponse(
             Guid.CreateVersion7(),
             clientId,
             20.0m,
             EnumPaymentMethod.PIX);

        return payment;
    }
}
