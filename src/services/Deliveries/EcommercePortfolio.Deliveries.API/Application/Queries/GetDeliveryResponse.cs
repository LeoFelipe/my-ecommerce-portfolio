using EcommercePortfolio.Deliveries.API.Application.Dtos;
using EcommercePortfolio.Deliveries.Domain.Entities;
using EcommercePortfolio.Deliveries.Domain.Enums;

namespace EcommercePortfolio.Deliveries.API.Application.Queries;

public record GetDeliveryResponse(
    Guid Id,
    Guid OrderId,
    Guid ClientId,
    DateTime ExpectedDate,
    EnumDeliveryStatus DeliveryStatus,
    AddressDto Address)
{
    public static explicit operator GetDeliveryResponse(Delivery delivery)
    {
        if (delivery == null || delivery.Id == Guid.Empty)
            return null;

        return new GetDeliveryResponse(
            delivery.Id,
            delivery.OrderId,
            delivery.ClientId,
            delivery.ExpectedDate,
            delivery.DeliveryStatus,
            (AddressDto)delivery.Address);
    }
}

public static class GetDeliveryResponseExtensions
{
    public static IReadOnlyCollection<GetDeliveryResponse> MapToDeliveriesList(this IEnumerable<Delivery> deliveries)
        => [.. deliveries.Select(delivery => (GetDeliveryResponse)delivery)];
}
