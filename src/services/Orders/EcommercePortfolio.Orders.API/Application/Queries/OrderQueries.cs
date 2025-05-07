using EcommercePortfolio.Orders.Domain;
using EcommercePortfolio.Services.Caching;

namespace EcommercePortfolio.Orders.API.Application.Queries;

public interface IOrderQueries
{
    Task<GetOrderResponse> GetById(Guid id);
    Task<GetAddressOrderResponse> GetAddressById(Guid id);
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
        return await _redisRepository.GetData(
            () => GetByIdCache(id),
            $"order:{id}");
    }

    public async Task<GetAddressOrderResponse> GetAddressById(Guid id)
    {
        return await _redisRepository.GetData(
            () => GetAddressByIdCache(id),
            $"order:{id}:address");
    }

    public async Task<IReadOnlyCollection<GetOrderResponse>> GetByClientId(Guid clientId)
    {
        return await _redisRepository.GetData(
            () => GetByClientIdCache(clientId),
            $"order:client:{clientId}");
    }

    private async Task<GetOrderResponse> GetByIdCache(Guid id)
    {
        var order = await _orderRepository.GetById(id);
        return (GetOrderResponse)order;
    }

    private async Task<GetAddressOrderResponse> GetAddressByIdCache(Guid id)
    {
        var order = await _orderRepository.GetById(id);
        return (GetAddressOrderResponse)order?.Address;
    }

    private async Task<IReadOnlyCollection<GetOrderResponse>> GetByClientIdCache(Guid clientId)
    {
        var orders = await _orderRepository.GetByClientId(clientId);
        return orders.MapToOrdersList();
    }
}
