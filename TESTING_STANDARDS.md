# 🧪 Testing Standards - Ecommerce Portfolio

## 📋 Overview

This document defines the testing standards for the Ecommerce Portfolio project, ensuring consistency and quality across all test implementations.

### Test Categories

1. **Unit Tests** (`EcommercePortfolio.*.UnitTests`)
   - Test individual components in isolation
   - Use mocks for external dependencies
   - Fast execution, no infrastructure required

2. **Functional Tests** (`EcommercePortfolio.FunctionalTests`)
   - Test complete service workflows
   - Use Testcontainers for infrastructure
   - Verify cross-service communication

## 📝 Naming Conventions

### 1. Test Method Pattern
```csharp
public async Task [Feature]_[Action]_Should[ExpectedResult]_When[Condition]()
```

#### Components:
- **Feature**: Entity/Service being tested (Order, Cart, Payment)
- **Action**: Operation being performed (Create, Get, Update, Delete)
- **Should[ExpectedResult]**: Expected outcome, always starting with "Should"
- **When[Condition]**: Test scenario or condition

#### Examples:
```csharp
// Functional Tests
public async Task Order_Create_ShouldCreateOrderAndDeliveryAndDeleteCart_WhenValidCartExists()
public async Task Cart_Update_ShouldUpdateCartItems_WhenItemsAreModified()
public async Task Payment_Process_ShouldAuthorizePayment_WhenValidPaymentMethodProvided()

// Unit Tests
public void Order_Create_ShouldCreateOrderWithValidProperties_WhenAllRequiredDataProvided()
public void Order_Validate_ShouldReturnValidationError_WhenTotalValueIsInvalid()
public async Task OrderCommand_Handle_ShouldPersistOrderAndReturnSuccess_WhenValidCommandReceived()
```

### 2. Project Structure

```
tests/
├── EcommercePortfolio.FunctionalTests/
│   ├── OrderFunctionalTests/
│   │   └── OrderFunctionalTests.cs
│   ├── CartFunctionalTests/
│   │   └── CartFunctionalTests.cs
│   ├── Factories/
│   │   ├── Configurations/
│   │   │   └── BuilderContainerFactory.cs
│   │   ├── OrderFactory.cs
│   │   └── CartFactory.cs
│   └── AssertsHelper/
│       ├── MongoAssertHelper.cs
│       └── PostgresAssertHelper.cs
│
└── EcommercePortfolio.[Service].UnitTests/
    ├── EntitiesTests/
    │   └── [Entity]Tests.cs
    ├── CommandsTests/
    │   └── [Command]Tests.cs
    ├── ControllersTests/
    │   └── [Controller]Tests.cs
    └── Factories/
        └── [Entity]Factory.cs
```

## 🏗️ Implementation Patterns

### 1. Functional Tests
```csharp
public class OrderFunctionalTests : IAsyncLifetime
{
    // 1. Infrastructure
    private readonly PostgresContainer _postgresContainer;
    private readonly MongoContainer _mongoContainer;
    private readonly RabbitMqContainer _rabbitMqContainer;
    
    // 2. API Clients
    private readonly WebApplicationFactory<Orders.Program> _ordersFactory;
    private readonly WebApplicationFactory<Carts.Program> _cartsFactory;
    private HttpClient _ordersHttpClient;
    private HttpClient _cartsHttpClient;
    
    // 3. Connections
    private string _postgresConnection;
    private string _mongoConnection;
    
    // 4. Setup/Teardown
    public async Task InitializeAsync()
    {
        // Start containers
        // Configure clients
        // Setup connections
    }
    
    public async Task DisposeAsync()
    {
        // Cleanup containers
        // Dispose clients
    }
    
    // 5. Tests
    [Fact]
    public async Task Order_Create_ShouldCreateOrderAndDeliveryAndDeleteCart_WhenValidCartExists()
    {
        // Arrange
        var clientId = Guid.CreateVersion7();
        
        // Act
        await CartFactory.PostCart(_cartsHttpClient, clientId);
        var cart = await CartFactory.GetCartByClientIdAsync(_cartsHttpClient, clientId);
        await OrderFactory.PostOrder(_ordersHttpClient, cart.Id, clientId);
        
        // Assert
        await MongoAssertHelper.AssertCartDoesNotExistAsync(_mongoConnection, clientId);
        await PostgresAssertHelper.AssertOrderExistsAsync(_postgresConnection, clientId);
        await PostgresAssertHelper.AssertDeliveryExistsAsync(_postgresConnection, clientId);
    }
}
```

### 2. Unit Tests
```csharp
public class OrderTests
{
    // 1. Dependencies
    private readonly Mock<IPaymentService> _paymentServiceMock;
    private readonly Mock<ICartService> _cartServiceMock;
    
    // 2. Setup
    public OrderTests()
    {
        _paymentServiceMock = new Mock<IPaymentService>();
        _cartServiceMock = new Mock<ICartService>();
    }
    
    // 3. Tests
    [Fact]
    public void Order_Create_ShouldCreateOrderWithValidProperties_WhenAllRequiredDataProvided()
    {
        // Arrange
        var clientId = Guid.CreateVersion7();
        var orderData = OrderFactory.BuildValidOrderData(clientId);
        
        // Act
        var order = Order.Create(orderData);
        
        // Assert
        order.Should().NotBeNull();
        order.ClientId.Should().Be(clientId);
        order.Status.Should().Be(OrderStatus.Created);
        order.OrderItems.Should().HaveCount(orderData.Items.Count);
    }
}
```

## 🏭 Factories

### 1. Entity Factory Pattern
```csharp
public static class OrderFactory
{
    private static readonly Faker _faker = new("en");
    
    public static OrderData BuildValidOrderData(Guid clientId)
    {
        return new OrderData
        {
            ClientId = clientId,
            Items = new List<OrderItemData>
            {
                new()
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = _faker.Random.Int(1, 5),
                    UnitPrice = _faker.Random.Decimal(10, 100)
                }
            }
        };
    }
    
    public static OrderData BuildOrderDataWithInvalidTotal()
    {
        var data = BuildValidOrderData(Guid.NewGuid());
        data.Items.First().UnitPrice = -1;
        return data;
    }
}
```

### 2. API Factory Pattern
```csharp
public static class OrderApiFactory
{
    public static async Task<OrderResponse> PostOrder(
        HttpClient client,
        Guid cartId,
        Guid clientId)
    {
        var request = new CreateOrderRequest
        {
            CartId = cartId,
            ClientId = clientId
        };
        
        var response = await client.PostAsJsonAsync("/api/orders", request);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<OrderResponse>();
    }
}
```

## ✅ Assertions

### 1. Fluent Assertions
```csharp
// Entity assertions
order.Should().NotBeNull();
order.ClientId.Should().Be(clientId);
order.Status.Should().Be(OrderStatus.Created);
order.OrderItems.Should().HaveCount(1);

// Collection assertions
orders.Should().NotBeEmpty();
orders.Should().Contain(o => o.ClientId == clientId);
orders.Should().BeInDescendingOrder(o => o.CreatedAt);

// Exception assertions
var act = () => Order.Create(invalidData);
act.Should().Throw<DomainException>()
   .WithMessage("Total value must be greater than zero");
```

## 🎯 Best Practices

### 1. Test Organization
- One test class per entity/feature
- Group related tests using nested classes
- Use meaningful test data
- Follow AAA pattern consistently

### 2. Test Data
- Use Bogus for generating test data
- Keep factories in dedicated classes
- Avoid magic numbers/strings
- Use constants for common values

### 3. Assertions
- Use FluentAssertions for readable assertions
- Be specific in error messages
- Verify only what's necessary
- Use appropriate assertion methods

### 4. Infrastructure
- Use Testcontainers for functional tests
- Clean up resources in DisposeAsync
- Handle container lifecycle properly
- Use appropriate database assertions

### 5. Code Coverage
- Aim for high coverage of business logic
- Test success and error cases
- Test domain validations
- Test service integrations

### 6. Performance
- Keep tests fast and focused
- Use appropriate test categories
- Avoid unnecessary setup
- Clean up resources properly

## 🔍 Common Test Scenarios

### 1. Entity Creation
```csharp
[Fact]
public void Order_Create_ShouldCreateOrderWithValidProperties_WhenAllRequiredDataProvided()
{
    // Arrange
    var orderData = OrderFactory.BuildValidOrderData(clientId);
    
    // Act
    var order = Order.Create(orderData);
    
    // Assert
    order.Should().NotBeNull();
    order.ClientId.Should().Be(clientId);
    order.Status.Should().Be(OrderStatus.Created);
}
```

### 2. Validation
```csharp
[Fact]
public void Order_Validate_ShouldReturnValidationError_WhenTotalValueIsInvalid()
{
    // Arrange
    var orderData = OrderFactory.BuildOrderDataWithInvalidTotal();
    
    // Act
    var act = () => Order.Create(orderData);
    
    // Assert
    act.Should().Throw<DomainException>()
       .WithMessage("Total value must be greater than zero");
}
```

## 📚 References

- [xUnit Documentation](https://xunit.net/)
- [FluentAssertions](https://fluentassertions.com/)
- [Bogus](https://github.com/bchavez/Bogus)
- [Testcontainers for .NET](https://github.com/testcontainers/testcontainers-dotnet)
- [MassTransit Testing](https://masstransit.io/documentation/patterns/testing)