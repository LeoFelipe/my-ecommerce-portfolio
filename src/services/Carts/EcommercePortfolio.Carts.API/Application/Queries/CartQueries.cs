using EcommercePortfolio.Carts.Domain.Carts;
using EcommercePortfolio.Services.Caching;

namespace EcommercePortfolio.Carts.API.Application.Queries;

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
        return await _redisRepository.GetData(
            () => GetByClientIdCache(clientId),
            $"cart:client:{clientId}");
    }

    private async Task<GetCartResponse> GetByClientIdCache(Guid clientId)
    {
        var cart = await _cartRepository.GetByClientId(clientId);
        return (GetCartResponse)cart;
    }
}
