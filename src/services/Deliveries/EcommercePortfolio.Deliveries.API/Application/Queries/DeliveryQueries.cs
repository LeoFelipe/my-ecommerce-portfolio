using EcommercePortfolio.Deliveries.Domain;
using EcommercePortfolio.Services.Caching;

namespace EcommercePortfolio.Deliveries.API.Application.Queries;

public interface IDeliveryQueries
{
    Task<GetDeliveryResponse> GetByOrderId(Guid orderId);
}

public class DeliveryQueries(
    IDeliveryRepository deliveryRepository,
    IRedisRepository redisRepository) : IDeliveryQueries
{
    private readonly IDeliveryRepository _deliveryRepository = deliveryRepository;
    private readonly IRedisRepository _redisRepository = redisRepository;

    public async Task<GetDeliveryResponse> GetByOrderId(Guid orderId)
    {
        var delivery = await _redisRepository.GetData(
            () => _deliveryRepository.GetByOrderId(orderId),
            $"delivery:order:{orderId}");

        return delivery != null
            ? (GetDeliveryResponse)delivery
            : null;
    }
}
