using Bogus;
using EcommercePortfolio.Orders.Domain.Entities;

namespace EcommercePortfolio.Orders.UnitTests.Factories.Orders;

public static class OrderEntityFactory
{
    private static readonly Faker _faker = new("en");

    public static Order BuildValidOrder(Guid? clientId = null, int quantity = 10, decimal price = 50)
    {
        var order = Order.CreateOrder(
            clientId ?? Guid.NewGuid(),
            [
                BuildOrderItem(
                    _faker.Commerce.Random.Number(100, 1000),
                    _faker.Commerce.ProductName(),
                    _faker.Commerce.Categories(1)[0],
                    quantity,
                    price)
            ]);

        order.SetAddress(Core.Domain.ValueObjects.Address.CreateAddress(
            _faker.Address.ZipCode(),
            _faker.Address.State(),
            _faker.Address.City(),
            _faker.Address.StreetAddress(),
            _faker.Commerce.Random.Number(1001, 9999)));

        return order;
    }

    public static Order BuildWithoutClientId()
    {
        return Order.CreateOrder(
            Guid.Empty,
            [
                BuildValidOrderItem()
            ]);
    }

    public static Order BuildWithoutAddress()
    {
        return Order.CreateOrder(
            Guid.NewGuid(),
            [
                BuildValidOrderItem()
            ]);
    }

    public static Order BuildWithoutItems()
    {
        var order = Order.CreateOrder(Guid.NewGuid(), []);

        order.SetAddress(Core.Domain.ValueObjects.Address.CreateAddress(
            _faker.Address.ZipCode(),
            _faker.Address.State(),
            _faker.Address.City(),
            _faker.Address.StreetAddress(),
            _faker.Commerce.Random.Number(1001, 9999)));

        return order;
    }

    public static Order BuildOrder(
        int productId = 0,
        string? productName = null,
        string? category = null,
        int quantity = 0,
        decimal price = 0m)
    {
        var order = Order.CreateOrder(
            Guid.NewGuid(),
            [
                BuildOrderItem(
                    productId,
                    productName,
                    category,
                    quantity,
                    price)
            ]);

        order.SetAddress(Core.Domain.ValueObjects.Address.CreateAddress(
            _faker.Address.ZipCode(),
            _faker.Address.State(),
            _faker.Address.City(),
            _faker.Address.StreetAddress(),
            _faker.Commerce.Random.Number(1001, 9999)));

        return order;
    }

    public static OrderItem BuildValidOrderItem()
    {
        return OrderItem.CreateOrderItem(
            _faker.Commerce.Random.Number(100, 1000),
            _faker.Commerce.ProductName(),
            _faker.Commerce.Categories(1)[0],
            _faker.Commerce.Random.Number(1, 50),
            _faker.Random.Decimal(5m, 200m));
    }

    public static OrderItem BuildOrderItem(
        int productId = 0,
        string? productName = null,
        string? category = null,
        int quantity = 0,
        decimal price = 0m)
    {
        return OrderItem.CreateOrderItem(
            productId,
            productName,
            category,
            quantity,
            price);
    }
}
