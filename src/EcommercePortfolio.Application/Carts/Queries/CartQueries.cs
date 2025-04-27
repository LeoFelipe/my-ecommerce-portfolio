using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Domain.Caching;
using EcommercePortfolio.Domain.Carts;

namespace EcommercePortfolio.Application.Carts.Queries;

public interface ICartQueries
{
    Task<GetCartResponse?> GetByClientId(Guid clientId);
}

public class CartQueries(
    ICartRepository cartRepository,
    IRedisRepository redisRepository) : ICartQueries
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IRedisRepository _redisRepository = redisRepository;

    public async Task<GetCartResponse?> GetByClientId(Guid clientId)
    {
        var cart = await _redisRepository.GetData(
            () => _cartRepository.GetByClientId(clientId),
            $"cart:client:{clientId}");

        return cart != null 
            ? (GetCartResponse)cart 
            : null;
    }
}
