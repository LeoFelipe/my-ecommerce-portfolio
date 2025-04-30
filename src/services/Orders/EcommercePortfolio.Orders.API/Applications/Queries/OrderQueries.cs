using EcommercePortfolio.Orders.Domain;
using EcommercePortfolio.Services.Caching;

namespace EcommercePortfolio.Orders.API.Applications.Queries;

public interface IOrderQueries
{
    Task<GetOrderResponse> GetById(Guid id);
    Task<IReadOnlyCollection<GetOrderResponse>> GetByClientId(Guid clientId);
}
public class OrderQueries(
    IOrderRepository orderRepository,
    IRedisRepository redisRepository) : IOrderQueries
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IRedisRepository _redisRepository = redisRepository;

    public async Task<GetOrderResponse> GetById(Guid id)
    {
        var order = await _redisRepository.GetData(
            () => _orderRepository.GetById(id),
            $"order:{id}");

        return order != null
            ? (GetOrderResponse)order
            : null;
    }

    public async Task<IReadOnlyCollection<GetOrderResponse>> GetByClientId(Guid clientId)
    {
        var orders = await _redisRepository.GetData(
            () => _orderRepository.GetByClientId(clientId),
            $"order:client:{clientId}");

        return orders.MapToOrdersList();
    }
}
