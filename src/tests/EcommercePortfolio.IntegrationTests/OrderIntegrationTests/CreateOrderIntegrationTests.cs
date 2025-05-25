using EcommercePortfolio.IntegrationTests.Factories;
using EcommercePortfolio.IntegrationTests.Helpers.Asserts;

namespace EcommercePortfolio.IntegrationTests.OrderIntegrationTests;

public class CreateOrderIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task OrderIntegration_CreateOrder_ShouldCreateOrderAndCreateDeliveryAndDeleteCart()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act 1: Create the Cart
        await CartFactory.PostCart(_cartsHttpClient, clientId);
        var getCartResponse = await CartFactory.GetCartByClientIdAsync(_cartsHttpClient, clientId);

        // Act 2: Create the Order
        await OrderFactory.PostOrder(_ordersHttpClient, getCartResponse.Id, clientId);

        // Asserts
        await MongoAssertHelper.AssertCartDoesNotExistAsync(_cartMongoConnectionString, clientId);
        await PostgresAssertHelper.AssertOrderExistsAsync(_orderPostgresConnectionString, clientId);
        await PostgresAssertHelper.AssertDeliveryExistsAsync(_deliveryPostgresConnectionString, clientId);
    }
}
