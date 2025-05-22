# Testing Standards - Ecommerce Portfolio

## 📋 Naming Conventions

### 1. General Test Naming Pattern
```csharp
public async Task [Feature]_[Action]_[ExpectedResult]()
```

#### Components:
- **Feature**: Name of the functionality/entity being tested (e.g., Order, Cart, Payment)
- **Action**: Action being executed (e.g., Create, Get, Update, Delete)
- **ExpectedResult**: Expected result, always starting with "Should"

#### Examples:
```csharp
// Integration Tests
public async Task Order_Create_ShouldCreateOrderAndDeliveryAndDeleteCart()
public async Task Cart_Update_ShouldUpdateCartItems()
public async Task Payment_Process_ShouldAuthorizePayment()

// Unit Tests
public void Order_Create_ShouldCreateOrderSuccessfully()
public void Order_Validate_ShouldReturnErrorWhenTotalValueInvalid()
public async Task OrderCommand_Handle_ShouldPersistOrder()
```

### 2. File Organization

#### Directory Structure:
```
tests/
├── EcommercePortfolio.IntegrationTests/
│   ├── [Feature]IntegrationTests/
│   │   └── [Feature]IntegrationTests.cs
│   ├── Factories/
│   │   └── [Feature]Factory.cs
│   └── AssertsHelper/
│       └── [Database]AssertHelper.cs
│
└── EcommercePortfolio.[Feature].UnitTests/
    ├── EntitiesTests/
    │   └── [Entity]Tests.cs
    ├── CommandsTests/
    │   └── [Command]Tests.cs
    ├── ControllersTests/
    │   └── [Controller]Tests.cs
    └── Factories/
        └── [Feature]/
            └── [Entity]Factory.cs
```

### 3. Implementation Patterns

#### 3.1. Integration Tests
```csharp
public class [Feature]IntegrationTests : IAsyncLifetime
{
    // 1. Containers
    private readonly [Database]Container _[database]Container;
    
    // 2. Factories
    private readonly WebApplicationFactory<[API].Program> _[api]Factory;
    
    // 3. HTTP Clients
    private HttpClient _[api]HttpClient;
    
    // 4. Setup/Teardown
    public async Task InitializeAsync() { ... }
    public async Task DisposeAsync() { ... }
    
    // 5. Tests
    [Fact]
    public async Task [Feature]_[Action]_[ExpectedResult]()
    {
        // Arrange
        // Act
        // Assert
    }
}
```

#### 3.2. Unit Tests
```csharp
public class [Entity]Tests
{
    // 1. Dependencies (if needed)
    private readonly Mock<[Interface]> _[interface]Mock;
    
    // 2. Setup (if needed)
    public [Entity]Tests()
    {
        _[interface]Mock = new Mock<[Interface]>();
    }
    
    // 3. Tests
    [Fact]
    public void [Feature]_[Action]_[ExpectedResult]()
    {
        // Arrange
        // Act
        // Assert
    }
}
```

### 4. Factories

#### 4.1. Factory Pattern
```csharp
public static class [Entity]Factory
{
    private static readonly Faker _faker = new("en");
    
    // Creation methods
    public static [Entity] BuildValid[Entity]()
    public static [Entity] Build[Entity]With[Condition]()
    public static [Entity] BuildInvalid[Entity]()
}
```

#### 4.2. Factory Method Naming
- `BuildValid[Entity]()` - Creates a valid entity
- `Build[Entity]With[Condition]()` - Creates an entity with specific condition
- `BuildInvalid[Entity]()` - Creates an invalid entity

### 5. Assertions

#### 5.1. Assertion Helper Pattern
```csharp
public static class [Database]AssertHelper
{
    public static async Task Assert[Entity]ExistsAsync(string connectionString, ...)
    public static async Task Assert[Entity]DoesNotExistAsync(string connectionString, ...)
    public static async Task Assert[Entity]Has[Property]Async(string connectionString, ...)
}
```

### 6. Best Practices

1. **AAA Pattern**
   - Arrange: Prepare the scenario
   - Act: Execute the action
   - Assert: Verify the result

2. **Isolation**
   - Each test should be independent
   - Use isolated containers for integration tests
   - Clean up data after each test

3. **Test Data**
   - Use Bogus for fake data
   - Keep factories organized
   - Avoid hardcoded data

4. **Assertions**
   - Use FluentAssertions for expressive assertions
   - Be specific in error messages
   - Verify only what's necessary

5. **Coverage**
   - Test success and error cases
   - Test domain validations
   - Test service integrations

### 7. Implementation Examples

#### 7.1. Integration Test
```csharp
[Fact]
public async Task Order_Create_ShouldCreateOrderAndDeliveryAndDeleteCart()
{
    // Arrange
    var clientId = Guid.NewGuid();
    
    // Act
    await CartFactory.PostCart(_cartsHttpClient, clientId);
    var cart = await CartFactory.GetCartByClientIdAsync(_cartsHttpClient, clientId);
    await OrderFactory.PostOrder(_ordersHttpClient, cart.Id, clientId);
    
    // Assert
    await MongoAssertHelper.AssertCartDoesNotExistAsync(_mongoConnection, clientId);
    await PostgresAssertHelper.AssertOrderExistsAsync(_postgresConnection, clientId);
    await PostgresAssertHelper.AssertDeliveryExistsAsync(_postgresConnection, clientId);
}
```

#### 7.2. Unit Test
```csharp
[Fact]
public void Order_Create_ShouldCreateOrderSuccessfully()
{
    // Arrange
    var clientId = Guid.NewGuid();
    
    // Act
    var order = OrderEntityFactory.BuildValidOrder(clientId);
    
    // Assert
    order.Should().NotBeNull();
    order.ClientId.Should().Be(clientId);
    order.TotalValue.Should().Be(500.0m);
    order.OrderItems.Should().HaveCount(1);
} 