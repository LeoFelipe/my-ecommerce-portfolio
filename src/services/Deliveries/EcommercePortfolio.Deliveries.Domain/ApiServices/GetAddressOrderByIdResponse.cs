using EcommercePortfolio.Core.Domain.ValueObjects;

namespace EcommercePortfolio.Deliveries.Domain.ApiServices;

public record GetAddressOrderByIdResponse(
    string ZipCode,
    string State,
    string City,
    string StreetAddress,
    int NumberAddress)
{
    public static explicit operator Address(GetAddressOrderByIdResponse address)
    {
        if (address == null)
            return null;

        return Address.CreateAddress(
            address.ZipCode,
            address.State,
            address.City,
            address.StreetAddress,
            address.NumberAddress);
    }
}
