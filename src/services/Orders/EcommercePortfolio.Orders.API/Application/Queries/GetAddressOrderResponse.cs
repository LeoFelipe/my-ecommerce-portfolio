using EcommercePortfolio.Core.Domain.ValueObjects;

namespace EcommercePortfolio.Orders.API.Application.Queries;

public record GetAddressOrderResponse(
    string ZipCode,
    string State,
    string City,
    string StreetAddress,
    int NumberAddress)
{
    public static explicit operator GetAddressOrderResponse(Address address)
    {
        if (address == null)
            return null;

        return new GetAddressOrderResponse(
            address.ZipCode,
            address.State,
            address.City,
            address.StreetAddress,
            address.NumberAddres);
    }
}