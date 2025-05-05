using EcommercePortfolio.Carts.Domain.Carts;
using EcommercePortfolio.Services.Caching;

namespace EcommercePortfolio.Carts.API.Applications.Queries;

public interface ICartQueries
{
    Task<GetCartResponse> GetByClientId(Guid clientId);
}

public class CartQueries(
    ICartRepository cartRepository,
    IRedisRepository redisRepository) : ICartQueries
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IRedisRepository _redisRepository = redisRepository;

    public async Task<GetCartResponse> GetByClientId(Guid clientId)
    {
        var cart = await _redisRepository.GetData(
            () => _cartRepository.GetByClientId(clientId),
            $"cart:client:{clientId}");

        return cart != null
            ? (GetCartResponse)cart
            : null;
    }
}
