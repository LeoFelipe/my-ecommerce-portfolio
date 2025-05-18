using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Orders.Domain.Entities;
using EcommercePortfolio.Orders.Domain.Enums;

namespace EcommercePortfolio.Orders.UnitTests.EntitiesTests;

public class OrderTests
{
    [Fact]
    public void CreateOrder_AllRequiredPropertiesValid_OrderCreatedSuccessfully()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var items = new List<OrderItem>
        {
            OrderItem.CreateOrderItem(1, "Product A", "Category A", 2, 10.0m)
        };

        // Act
        var order = Order.CreateOrder(clientId, items);

        // Assert
        Assert.NotNull(order);
        Assert.Equal(clientId, order.ClientId);
        Assert.Equal(20.0m, order.TotalValue);
        Assert.Single(order.OrderItems);
    }

    [Fact]
    public void CreateOrder_ClientIdInvalid_ThrowDomainException()
    {
        // Act + Assert
        var exception = Assert.Throws<DomainException>(() =>
            Order.CreateOrder(
                Guid.Empty,
                [
                    OrderItem.CreateOrderItem(1, "Product A", "Category A", 2, 10.0m)
                ])
        );

        Assert.Equal("Client id not informed", exception.Message);
    }

    [Fact]
    public void CreateOrder_EmptyItems_ThrowDomainException()
    {
        // Act + Assert
        var exception = Assert.Throws<DomainException>(() =>
            Order.CreateOrder(Guid.NewGuid(), [])
        );

        Assert.Equal("Order items not informed", exception.Message);
    }

    [Fact]
    public void CreateOrder_ZeroQuantityItems_ThrowDomainException()
    {
        // Act + Assert
        var exception = Assert.Throws<DomainException>(() =>
            Order.CreateOrder(
                Guid.NewGuid(),
                [
                    OrderItem.CreateOrderItem(1, "Product A", "Category A", 0, 10.0m)
                ])
        );

        Assert.Equal("Quantity not informed", exception.Message);
    }

    [Fact]
    public void CreateOrder_ZeroPriceItems_ThrowDomainException()
    {
        // Act + Assert
        var exception = Assert.Throws<DomainException>(() =>
            Order.CreateOrder(Guid.NewGuid(),
            [
                OrderItem.CreateOrderItem(1, "Product A", "Category A", 2, 0.0m)
            ])
        );

        Assert.Equal("Price not informed", exception.Message);
    }

    [Fact]
    public void Order_PaymentAuthorized_ShouldSetPaymentIdAndAuthorizeStatus()
    {
        // Act
        var order = Order.CreateOrder(
            Guid.NewGuid(),
            [
                OrderItem.CreateOrderItem(1, "Product A", "Category A", 2, 10.0m)
            ]);

        var paymentId = Guid.NewGuid();
        order.PaymentAuthorized(paymentId);

        // Assert
        Assert.Equal(paymentId, order.PaymentId);
        Assert.Equal(EnumOrderStatus.AUTHORIZED, order.OrderStatus);
    }

    [Fact]
    public void Order_Canceled_ShouldSetStatusToCanceled()
    {
        // Act
        var order = Order.CreateOrder(
            Guid.NewGuid(),
            [
                OrderItem.CreateOrderItem(1, "Product A", "Category A", 2, 10.0m)
            ]);

        order.Cancel();

        // Assert
        Assert.Equal(EnumOrderStatus.CANCELED, order.OrderStatus);
    }

    [Fact]
    public void Order_Approved_ShouldSetStatusToApproved()
    {
        // Act
        var order = Order.CreateOrder(
            Guid.NewGuid(),
            [
                OrderItem.CreateOrderItem(1, "Product A", "Category A", 2, 10.0m)
            ]);

        order.Approve();

        // Assert
        Assert.Equal(EnumOrderStatus.APPROVED, order.OrderStatus);
    }

    [Fact]
    public void ValidateForCreation_InvalidTotalValue_ShouldReturnMessageError()
    {
        // Act
        var order = Order.CreateOrder(
            Guid.NewGuid(),
            [
                OrderItem.CreateOrderItem(1, "Product A", "Category A", 2, 10.0m)
            ]);

        order.PaymentAuthorized(Guid.NewGuid());
        var error = order.ValidateForCreation(999.0m);

        // Assert
        Assert.Equal("The order total value is different from cart total value", error);
    }

    [Fact]
    public void ValidateForCreation_UnauthorizedOrder_ShouldReturnMessageError()
    {
        // Act
        var order = Order.CreateOrder(
            Guid.NewGuid(),
            [
                OrderItem.CreateOrderItem(1, "Product A", "Category A", 2, 10.0m)
            ]);

        var error = order.ValidateForCreation(order.TotalValue);

        // Assert
        Assert.Equal("The order is not authorized", error);
    }

    [Fact]
    public void ValidateForCreation_WithoutAddress_ShouldReturnMessageError()
    {
        // Act
        var order = Order.CreateOrder(
            Guid.NewGuid(),
            [
                OrderItem.CreateOrderItem(1, "Product A", "Category A", 2, 10.0m)
            ]);

        order.PaymentAuthorized(Guid.NewGuid());

        var error = order.ValidateForCreation(order.TotalValue);

        // Assert
        Assert.Equal("The address order is not informed", error);
    }
}
