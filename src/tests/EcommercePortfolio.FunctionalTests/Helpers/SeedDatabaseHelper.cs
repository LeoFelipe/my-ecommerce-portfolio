using EcommercePortfolio.Carts.Domain.Entities;
using EcommercePortfolio.Carts.Infra.Data;
using EcommercePortfolio.Orders.Domain.ApiServices;

namespace EcommercePortfolio.FunctionalTests.Helpers;

public static class SeedDatabaseHelper
{
    public static async Task<GetCartByClientIdResponse> InsertCartAndReturnCartResponse(MongoDbContext mongoDbContext, Cart cart)
    {
        await mongoDbContext.Carts.AddAsync(cart);

        return new GetCartByClientIdResponse(
            cart.Id.ToString(),
            cart.ClientId,
            cart.CartDate,
            cart.TotalValue,
            [.. cart.CartItems.Select(x => new CartItemDto(x.ProductId, x.ProductName, x.Category, x.Quantity, x.Price))]
        );
    }
}
