namespace EcommercePortfolio.Domain.Payments;

public interface IPaymentService
{
    // Quando criar a camada de Application remover esse Sevice, pois será feito no CommandHandler
    Task DoPayment(Payment payment);
}
