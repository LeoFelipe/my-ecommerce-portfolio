using EcommercePortfolio.FunctionalTests.Factories;
using EcommercePortfolio.FunctionalTests.Factories.Configurations;
using EcommercePortfolio.Orders.Domain.ApiServices;
using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace EcommercePortfolio.FunctionalTests;

public class OrderFunctionalTest(
    OrderFunctionalTestWebApplicationFactory orderFactory,
    CartFunctionalTestWebApplicationFactory cartFactory) :
    IClassFixture<OrderFunctionalTestWebApplicationFactory>,
    IClassFixture<CartFunctionalTestWebApplicationFactory>
{
    private readonly HttpClient _ordersHttpClient = orderFactory.CreateClient();
    private readonly HttpClient _cartsHttpClient = cartFactory.CreateClient();

    [Fact]
    public async Task PostOrder_ShouldCreateOrderAndCreateDeliveryAndDeleteCart_WhenRequestIsValid()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act 1: Create the Cart
        await CartFactory.PostCart(_cartsHttpClient, clientId);
        var getCartResponse = await CartFactory.GetCartByClientIdAsync(_cartsHttpClient, clientId);

        orderFactory.CartApiServiceMock.Setup(x => x.GetCartByClientId(It.IsAny<Guid>())).ReturnsAsync(getCartResponse);

        // Act 2: Create the Order
        await OrderFactory.PostOrder(_ordersHttpClient, getCartResponse.Id, clientId);

        // Asserts
        var getCartNewResponse = await CartFactory.GetCartByClientIdAsync(_cartsHttpClient, clientId);
        getCartNewResponse.Should().BeNull();

        var getOrderResponse = await OrderFactory.GetOrderByClientId(_cartsHttpClient, clientId);
        getOrderResponse.Should().NotBeNull();

        var getDeliveryResponse = await DeliveryFactory.GetDeliveryByOrderId(_cartsHttpClient, getOrderResponse.Id);
        getDeliveryResponse.Should().NotBeNull();

        //await MongoAssertHelper.AssertCartDoesNotExistAsync(_cartMongoConnectionString, clientId);
        //await PostgresAssertHelper.AssertOrderExistsAsync(_orderPostgresConnectionString, clientId);
        //await PostgresAssertHelper.AssertDeliveryExistsAsync(_deliveryPostgresConnectionString, clientId);
    }
}
