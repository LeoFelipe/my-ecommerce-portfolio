using Bogus;
using EcommercePortfolio.Orders.Domain.ApiServices;
using MongoDB.Bson;

namespace EcommercePortfolio.Orders.UnitTests.Factories.Carts;

public static class CartApiServiceFactory
{
    private static readonly Faker _faker = new("en");

    public static GetCartByClientIdResponse BuildValidCart(int quantity = 10, decimal price = 50m)
    {
        return new GetCartByClientIdResponse(
            new ObjectId().ToString(),
            Guid.CreateVersion7(),
            DateTime.Now,
            quantity * price,
            [
                new (
                    _faker.Commerce.Random.Number(100, 1000),
                    _faker.Commerce.ProductName(),
                    _faker.Commerce.Categories(1)[0],
                    quantity,
                    price)
            ]
        );
    }
}
