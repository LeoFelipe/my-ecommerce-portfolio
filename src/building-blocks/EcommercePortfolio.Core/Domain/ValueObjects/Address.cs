using EcommercePortfolio.Core.Domain;

namespace EcommercePortfolio.Domain.ValueObjects;

public class Address
{
    public string ZipCode { get; private set; }
    public string State { get; private set; }
    public string City { get; private set; }
    public string StreetAddress { get; private set; }
    public int NumberAddres { get; private set; }

    protected Address()
    {
        
    }

    public static Address CreateAddress(string zipCode, string state, string city, string streetAddress, int numberAddress)
    {
        if (string.IsNullOrWhiteSpace(zipCode)) throw new DomainException("Zip code not informed");
        if (string.IsNullOrWhiteSpace(state)) throw new DomainException("State not informed");
        if (string.IsNullOrWhiteSpace(city)) throw new DomainException("City not informed");
        if (string.IsNullOrWhiteSpace(streetAddress)) throw new DomainException("Street address not informed");
        if (numberAddress <= 0) throw new DomainException("Number address not informed");
        
        return new Address
        {
            ZipCode = zipCode,
            State = state,
            City = city,
            StreetAddress = streetAddress,
            NumberAddres = numberAddress
        };
    }
}
