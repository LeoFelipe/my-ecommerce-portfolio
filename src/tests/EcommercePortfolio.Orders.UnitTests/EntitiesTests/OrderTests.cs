using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Orders.Domain.Entities;
using EcommercePortfolio.Orders.Domain.Enums;
using EcommercePortfolio.Orders.UnitTests.Factories.Orders;

namespace EcommercePortfolio.Orders.UnitTests.EntitiesTests;

public class OrderTests
{
    [Fact]
    public void CreateOrder_AllRequiredPropertiesValid_OrderCreatedSuccessfully()
    {
        // Arrange + Act
        var clientId = Guid.NewGuid();

        // Act
        var order = OrderEntityFactory.BuildValidOrder(clientId);

        // Assert
        Assert.NotNull(order);
        Assert.Equal(clientId, order.ClientId);
        Assert.Equal(500.0m, order.TotalValue);
        Assert.Single(order.OrderItems);
    }

    [Fact]
    public void CreateOrder_ClientIdInvalid_ThrowDomainException()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildWithoutClientId()
        );

        // Assert
        Assert.Equal("Client id not informed", exception.Message);
    }

    [Fact]
    public void CreateOrder_EmptyItems_ThrowDomainException()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildWithoutItems()
        );

        // Assert
        Assert.Equal("Order items not informed", exception.Message);
    }

    [Fact]
    public void CreateOrder_ZeroQuantityItems_ThrowDomainException()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrder(1, "Product A", "Category A", 0, 10.0m)
        );

        // Assert
        Assert.Equal("Quantity not informed", exception.Message);
    }

    [Fact]
    public void CreateOrder_ZeroPriceItems_ThrowDomainException()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrder(1, "Product A", "Category A", 2, 0.0m)
        );

        // Assert
        Assert.Equal("Price not informed", exception.Message);
    }

    [Fact]
    public void Order_PaymentAuthorized_ShouldSetPaymentIdAndAuthorizeStatus()
    {
        // Arrange
        var order = OrderEntityFactory.BuildValidOrder();
        var paymentId = Guid.NewGuid();

        // Act
        order.PaymentAuthorized(paymentId);

        // Assert
        Assert.Equal(paymentId, order.PaymentId);
        Assert.Equal(EnumOrderStatus.AUTHORIZED, order.OrderStatus);
    }

    [Fact]
    public void Order_Canceled_ShouldSetStatusToCanceled()
    {
        // Arrange
        var order = OrderEntityFactory.BuildValidOrder();

        // Act
        order.Cancel();

        // Assert
        Assert.Equal(EnumOrderStatus.CANCELED, order.OrderStatus);
    }

    [Fact]
    public void Order_Approved_ShouldSetStatusToApproved()
    {
        // Arrange
        var order = OrderEntityFactory.BuildValidOrder();

        // Act
        order.Approve();

        // Assert
        Assert.Equal(EnumOrderStatus.APPROVED, order.OrderStatus);
    }

    [Fact]
    public void ValidateForCreation_InvalidTotalValue_ShouldReturnMessageError()
    {
        // Arrange
        var order = OrderEntityFactory.BuildValidOrder();
        order.PaymentAuthorized(Guid.NewGuid());

        // Act
        var error = order.ValidateForCreation(999.0m);

        // Assert
        Assert.Equal("The order total value is different from cart total value", error);
    }

    [Fact]
    public void ValidateForCreation_UnauthorizedOrder_ShouldReturnMessageError()
    {
        // Arrange
        var order = OrderEntityFactory.BuildValidOrder();

        // Act
        var error = order.ValidateForCreation(order.TotalValue);

        // Assert
        Assert.Equal("The order is not authorized", error);
    }

    [Fact]
    public void ValidateForCreation_WithoutAddress_ShouldReturnMessageError()
    {
        // Arrange
        var order = OrderEntityFactory.BuildWithoutAddress();
        order.PaymentAuthorized(Guid.NewGuid());

        // Act
        var error = order.ValidateForCreation(order.TotalValue);

        // Assert
        Assert.Equal("The address order is not informed", error);
    }
}
