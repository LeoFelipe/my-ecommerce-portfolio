using EcommercePortfolio.Carts.Domain.Entities;
using EcommercePortfolio.FunctionalTests.Factories;
using EcommercePortfolio.FunctionalTests.Factories.Configurations;
using EcommercePortfolio.FunctionalTests.Helpers;
using FluentAssertions;
using Moq;
using System.Net;

namespace EcommercePortfolio.FunctionalTests;

public class OrderFunctionalTest(
    OrderFunctionalTestWebApplicationFactory orderFactory) : IClassFixture<OrderFunctionalTestWebApplicationFactory>
{
    private readonly HttpClient _ordersHttpClient = orderFactory.CreateClient();

    [Fact]
    public async Task Order_Create_ShouldCreateOrderAndDeliveryAndDeleteCart_WhenRequestIsValid()
    {
        // Arrange
        var cart = new Cart(
            Guid.CreateVersion7(),
            [
                new(1, "Product 01", "Category 01", 10, 1),
                new(2, "Product 02", "Category 02", 20, 2)
            ]);

        var getCartResponse = await SeedDatabaseHelper.InsertCartAndReturnCartResponse(orderFactory.MongoDbContext, cart);

        orderFactory.CartApiServiceMock.Setup(x => x.GetCartByClientId(It.IsAny<Guid>())).ReturnsAsync(getCartResponse);

        // Act
        var response = await OrderFactory.PostOrder(_ordersHttpClient, getCartResponse.Id.ToString(), cart.ClientId);

        // Asserts
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
