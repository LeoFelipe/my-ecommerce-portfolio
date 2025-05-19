using Bogus;
using EcommercePortfolio.ExternalServices.MyFakePay.Enums;
using EcommercePortfolio.ExternalServices.MyFakePayApi;

namespace EcommercePortfolio.Orders.UnitTests.Factories.Payments;

public static class PaymentApiServiceFactory
{
    public static PaymentApiResponse BuildValidPayment(Guid clientId, decimal paymentTotalValue)
    {
        var payment = new PaymentApiResponse(
             Guid.NewGuid(),
             clientId,
             paymentTotalValue,
             EnumPaymentMethod.PIX);

        payment.AuthorizePayment();

        return payment;
    }

    public static PaymentApiResponse BuildPaymentNotAuthorized(Guid clientId)
    {
        var payment = new PaymentApiResponse(
             Guid.NewGuid(),
             clientId,
             20.0m,
             EnumPaymentMethod.PIX);

        return payment;
    }
}
