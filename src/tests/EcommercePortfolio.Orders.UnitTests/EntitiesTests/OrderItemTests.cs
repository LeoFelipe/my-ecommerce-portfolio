using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Orders.Domain.Entities;

namespace EcommercePortfolio.Orders.UnitTests.EntitiesTests;

public class OrderItemTests
{
    [Fact]
    public void CreateOrderItem_AllRequiredPropertiesValid_OrderItemCreatedSuccessfully()
    {
        // Act
        var orderItem = OrderItem.CreateOrderItem(1, "Product A", "Category A", 2, 10.0m);

        // Assert
        Assert.NotNull(orderItem);
    }

    [Fact]
    public void CreateOrderItem_ProductIdInvalid_ThrowDomainException()
    {
        // Act 
        var exception = Assert.Throws<DomainException>(() =>
            OrderItem.CreateOrderItem(0, "Product A", "Category A", 2, 10.0m)
        );

        // Assert
        Assert.Equal("Product id not informed", exception.Message);
    }

    [Fact]
    public void CreateOrderItem_ProductNameInvalid_ThrowDomainException()
    {
        // Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderItem.CreateOrderItem(1, "", "Category A", 2, 10.0m)
        );

        // Assert
        Assert.Equal("Product name not informed", exception.Message);
    }

    [Fact]
    public void CreateOrderItem_CategoryInvalid_ThrowDomainException()
    {
        // Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderItem.CreateOrderItem(1, "Product A", "", 2, 10.0m)
        );

        // Assert
        Assert.Equal("Category not informed", exception.Message);
    }

    [Fact]
    public void CreateOrderItem_QuantityInvalid_ThrowDomainException()
    {
        // Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderItem.CreateOrderItem(1, "Product A", "Category A", 0, 10.0m)
        );

        // Assert
        Assert.Equal("Quantity not informed", exception.Message);
    }

    [Fact]
    public void CreateOrderItem_PriceInvalid_ThrowDomainException()
    {
        // Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderItem.CreateOrderItem(1, "Product A", "Category A", 2, 0m)
        );

        // Assert
        Assert.Equal("Price not informed", exception.Message);
    }
}
