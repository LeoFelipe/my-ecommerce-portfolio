using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Orders.Domain.Enums;
using EcommercePortfolio.Orders.UnitTests.Factories.Orders;
using FluentAssertions;

namespace EcommercePortfolio.Orders.UnitTests.EntitiesTests;

public class OrderTests
{
    [Fact]
    public void Order_Create_ShouldCreateOrderSuccessfully()
    {
        // Arrange + Act
        var clientId = Guid.NewGuid();
        var order = OrderEntityFactory.BuildValidOrder(clientId);

        // Assert
        order.Should().NotBeNull();
        order.ClientId.Should().Be(clientId);
        order.TotalValue.Should().Be(500.0m);
        order.OrderItems.Should().HaveCount(1);
    }

    [Fact]
    public void Order_Create_ShouldThrowExceptionWhenClientIdInvalid()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildWithoutClientId()
        );

        // Assert
        exception.Message.Should().Be("Client id not informed");
    }

    [Fact]
    public void Order_Create_ShouldThrowExceptionWhenItemsEmpty()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildWithoutItems()
        );

        // Assert
        exception.Message.Should().Be("Order items not informed");
    }

    [Fact]
    public void Order_Create_ShouldThrowExceptionWhenQuantityZero()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrder(1, "Product A", "Category A", 0, 10.0m)
        );

        // Assert
        exception.Message.Should().Be("Quantity not informed");
    }

    [Fact]
    public void Order_Create_ShouldThrowExceptionWhenPriceZero()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrder(1, "Product A", "Category A", 2, 0.0m)
        );

        // Assert
        exception.Message.Should().Be("Price not informed");
    }

    [Fact]
    public void Order_AuthorizePayment_ShouldSetPaymentIdAndStatus()
    {
        // Arrange
        var order = OrderEntityFactory.BuildValidOrder();
        var paymentId = Guid.NewGuid();

        // Act
        order.PaymentAuthorized(paymentId);

        // Assert
        order.PaymentId.Should().Be(paymentId);
        order.OrderStatus.Should().Be(EnumOrderStatus.AUTHORIZED);
    }

    [Fact]
    public void Order_Cancel_ShouldSetStatusToCanceled()
    {
        // Arrange
        var order = OrderEntityFactory.BuildValidOrder();

        // Act
        order.Cancel();

        // Assert
        order.OrderStatus.Should().Be(EnumOrderStatus.CANCELED);
    }

    [Fact]
    public void Order_Approve_ShouldSetStatusToApproved()
    {
        // Arrange
        var order = OrderEntityFactory.BuildValidOrder();

        // Act
        order.Approve();

        // Assert
        order.OrderStatus.Should().Be(EnumOrderStatus.APPROVED);
    }

    [Fact]
    public void Order_ValidateCreation_ShouldReturnErrorWhenTotalValueInvalid()
    {
        // Arrange
        var order = OrderEntityFactory.BuildValidOrder();
        order.PaymentAuthorized(Guid.NewGuid());

        // Act
        var error = order.ValidateForCreation(999.0m);

        // Assert
        error.Should().Be("The order total value is different from cart total value");
    }

    [Fact]
    public void Order_ValidateCreation_ShouldReturnErrorWhenUnauthorized()
    {
        // Arrange
        var order = OrderEntityFactory.BuildValidOrder();

        // Act
        var error = order.ValidateForCreation(order.TotalValue);

        // Assert
        error.Should().Be("The order is not authorized");
    }

    [Fact]
    public void Order_ValidateCreation_ShouldReturnErrorWhenAddressNotInformed()
    {
        // Arrange
        var order = OrderEntityFactory.BuildWithoutAddress();
        order.PaymentAuthorized(Guid.NewGuid());

        // Act
        var error = order.ValidateForCreation(order.TotalValue);

        // Assert
        error.Should().Be("The address order is not informed");
    }
}
