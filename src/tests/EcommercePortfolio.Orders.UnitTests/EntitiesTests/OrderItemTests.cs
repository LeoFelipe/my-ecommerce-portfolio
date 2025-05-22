using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Orders.UnitTests.Factories.Orders;
using FluentAssertions;

namespace EcommercePortfolio.Orders.UnitTests.EntitiesTests;

public class OrderItemTests
{
    [Fact]
    public void OrderItem_Create_ShouldCreateOrderItemSuccessfully()
    {
        // Arrange + Act
        var orderItem = OrderEntityFactory.BuildValidOrderItem();

        // Assert
        orderItem.Should().NotBeNull();
    }

    [Fact]
    public void OrderItem_Create_ShouldThrowExceptionWhenProductIdInvalid()
    {
        // Arrange + Act 
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrderItem(0, "Product A", "Category A", 2, 10.0m)
        );

        // Assert
        exception.Message.Should().Be("Product id not informed");
    }

    [Fact]
    public void OrderItem_Create_ShouldThrowExceptionWhenProductNameInvalid()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrderItem(1, "", "Category A", 2, 10.0m)
        );

        // Assert
        exception.Message.Should().Be("Product name not informed");
    }

    [Fact]
    public void OrderItem_Create_ShouldThrowExceptionWhenCategoryInvalid()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrderItem(1, "Product A", "", 2, 10.0m)
        );

        // Assert
        exception.Message.Should().Be("Category not informed");
    }

    [Fact]
    public void OrderItem_Create_ShouldThrowExceptionWhenQuantityInvalid()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrderItem(1, "Product A", "Category A", 0, 10.0m)
        );

        // Assert
        exception.Message.Should().Be("Quantity not informed");
    }

    [Fact]
    public void OrderItem_Create_ShouldThrowExceptionWhenPriceInvalid()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrderItem(1, "Product A", "Category A", 2, 0m)
        );

        // Assert
        exception.Message.Should().Be("Price not informed");
    }
}
