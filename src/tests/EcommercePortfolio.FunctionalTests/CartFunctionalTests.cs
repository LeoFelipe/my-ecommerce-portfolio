using EcommercePortfolio.FunctionalTests.Factories;
using EcommercePortfolio.FunctionalTests.Factories.Configurations;
using FluentAssertions;
using System.Net;

namespace EcommercePortfolio.FunctionalTests;

public class CartFunctionalTest(
    CartFunctionalTestWebApplicationFactory cartFactory) : IClassFixture<CartFunctionalTestWebApplicationFactory>
{
    private readonly HttpClient _cartsHttpClient = cartFactory.CreateClient();

    [Fact]
    public async Task Cart_Create_ShouldCreateCart_WhenRequestIsValid()
    {
        // Arrange
        var clientId = Guid.CreateVersion7();

        // Act
        var response = await CartFactory.PostCart(_cartsHttpClient, clientId);

        // Asserts
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Cart_Create_ShouldReturnUnprocessableEntity_WhenCartAlreadyExists()
    {
        // Arrange
        var clientId = Guid.CreateVersion7();

        await CartFactory.PostCart(_cartsHttpClient, clientId);

        // Act
        var response = await CartFactory.PostCart(_cartsHttpClient, clientId);

        // Asserts
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }
}
