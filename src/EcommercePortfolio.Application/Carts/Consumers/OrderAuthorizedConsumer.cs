using EcommercePortfolio.Core.Messaging.Integration;
using EcommercePortfolio.Domain.Carts;
using MassTransit;

namespace EcommercePortfolio.Application.Carts.Consumers;

public class OrderAuthorizedConsumer(
    ICartRepository cartRepository) : IConsumer<OrderAuthorizedIntegrationEvent>
{
    private readonly ICartRepository _cartRepository = cartRepository;

    public async Task Consume(ConsumeContext<OrderAuthorizedIntegrationEvent> context)
    {
        var cart = await _cartRepository.GetByClientId(context.Message.ClientId);

        if (cart != null)
        {
            _cartRepository.Remove(cart);
            await _cartRepository.UnitOfWork.Commit();
        }
    }
}