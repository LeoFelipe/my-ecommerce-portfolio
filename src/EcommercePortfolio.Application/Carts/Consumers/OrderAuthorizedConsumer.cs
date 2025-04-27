using EcommercePortfolio.Core.Messaging.Integrations;
using EcommercePortfolio.Domain.Carts;
using MassTransit;

namespace EcommercePortfolio.Application.Carts.Consumers;

public class OrderAuthorizedConsumer(
    ICartRepository cartRepository) : IConsumer<UpdateCartForOrderAuthorizedIntegrationMessage>
{
    private readonly ICartRepository _cartRepository = cartRepository;

    public async Task Consume(ConsumeContext<UpdateCartForOrderAuthorizedIntegrationMessage> context)
    {
        var cart = await _cartRepository.GetByClientId(context.Message.ClientId);

        if (cart != null)
        {
            _cartRepository.Remove(cart);
            await _cartRepository.UnitOfWork.Commit();
        }
    }
}