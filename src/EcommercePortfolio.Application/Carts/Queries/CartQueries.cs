using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Domain.Caching;
using EcommercePortfolio.Domain.Carts;

namespace EcommercePortfolio.Application.Carts.Queries;

public interface ICartQueries
{
    Task<GetCartResponse> GetById(string id);
    Task<GetCartResponse> GetByClientId(Guid clientId);
}

public class CartQueries(
    ICartRepository cartRepository,
    IRedisRepository redisRepository,
    INotificationContext notification) : ICartQueries
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IRedisRepository _redisRepository = redisRepository;
    private readonly INotificationContext _notification = notification;

    public async Task<GetCartResponse> GetById(string id)
    {
        var cart = await _redisRepository.GetData(
            () => _cartRepository.GetById(id),
            $"cart:id:{id}",
            TimeSpan.FromMinutes(5));

        if (cart != null) return (GetCartResponse)cart;

        _notification.Add(EnumNotificationType.NOT_FOUND_ERROR, $"Cart not found");
        return null;
    }

    public async Task<GetCartResponse> GetByClientId(Guid clientId)
    {
        var cart = await _redisRepository.GetData(
            () => _cartRepository.GetByClientId(clientId),
            $"cart:clientId:{clientId}");

        if (cart != null) return (GetCartResponse)cart;

        _notification.Add(EnumNotificationType.NOT_FOUND_ERROR, $"Cart not found");
        return null;
    }
}
