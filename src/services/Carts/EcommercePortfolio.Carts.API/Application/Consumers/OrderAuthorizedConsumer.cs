using EcommercePortfolio.Carts.Domain.Carts;
using EcommercePortfolio.Core.Messaging.Integrations;
using MassTransit;

namespace EcommercePortfolio.Carts.API.Application.Consumers;

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