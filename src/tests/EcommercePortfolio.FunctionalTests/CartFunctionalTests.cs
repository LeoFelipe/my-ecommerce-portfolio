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
    public async Task PostCart_ShouldCreateCart_WhenRequestIsValid()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act
        var response = await CartFactory.PostCart(_cartsHttpClient, clientId);

        // Asserts
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task PostCart_ShouldReturnsUnprocessableEntity_WhenCartAlreadyExists()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        await CartFactory.PostCart(_cartsHttpClient, clientId);

        // Act
        var response = await CartFactory.PostCart(_cartsHttpClient, clientId);

        // Asserts
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }
}
