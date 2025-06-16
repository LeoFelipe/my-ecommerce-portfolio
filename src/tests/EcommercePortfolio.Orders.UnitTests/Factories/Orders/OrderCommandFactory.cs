using Bogus;
using EcommercePortfolio.ApiGateways.MyFakePay.Enums;
using EcommercePortfolio.Orders.API.Application.Commands;
using EcommercePortfolio.Orders.API.Application.Dtos;

namespace EcommercePortfolio.Orders.UnitTests.Factories.Orders;

public static class OrderCommandFactory
{
    private static readonly Faker _faker = new("en");

    public static CreateOrderCommand BuildValidCreateOrderCommand(Guid? clientId = null)
    {
        return new CreateOrderCommand(
            _faker.Commerce.Random.AlphaNumeric(10),
            clientId ?? Guid.CreateVersion7(),
            EnumPaymentMethod.PIX,
            new AddressDto(
                _faker.Address.ZipCode(),
                _faker.Address.State(),
                _faker.Address.City(),
                _faker.Address.StreetAddress(),
                _faker.Commerce.Random.Number(1001, 9999)));
    }

    public static CreateOrderCommand BuildCreateOrderCommand(
        EnumPaymentMethod enumPaymentMethod,
        string cartId = null,
        Guid? clientId = null)
    {
        return new CreateOrderCommand(
            cartId,
            clientId ?? Guid.CreateVersion7(),
            enumPaymentMethod,
            new AddressDto(
                _faker.Address.ZipCode(),
                _faker.Address.State(),
                _faker.Address.City(),
                _faker.Address.StreetAddress(),
                _faker.Commerce.Random.Number(1001, 9999)));
    }

    public static CreateOrderCommand BuildValidCreateOrderCommandWithInvalidAddress()
    {
        return new CreateOrderCommand(
            _faker.Commerce.Random.AlphaNumeric(10),
            Guid.CreateVersion7(),
            _faker.PickRandom<EnumPaymentMethod>(),
            new AddressDto("", "", "", "", 0));
    }
}
