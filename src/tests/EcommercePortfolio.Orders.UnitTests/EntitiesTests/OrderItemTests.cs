using EcommercePortfolio.Core.Domain;
using EcommercePortfolio.Orders.UnitTests.Factories.Orders;

namespace EcommercePortfolio.Orders.UnitTests.EntitiesTests;

public class OrderItemTests
{
    [Fact]
    public void CreateOrderItem_AllRequiredPropertiesValid_OrderItemCreatedSuccessfully()
    {
        // Arrange + Act
        var orderItem = OrderEntityFactory.BuildValidOrderItem();

        // Assert
        Assert.NotNull(orderItem);
    }

    [Fact]
    public void CreateOrderItem_ProductIdInvalid_ThrowDomainException()
    {
        // Arrange + Act 
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrderItem(0, "Product A", "Category A", 2, 10.0m)
        );

        // Assert
        Assert.Equal("Product id not informed", exception.Message);
    }

    [Fact]
    public void CreateOrderItem_ProductNameInvalid_ThrowDomainException()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrderItem(1, "", "Category A", 2, 10.0m)
        );

        // Assert
        Assert.Equal("Product name not informed", exception.Message);
    }

    [Fact]
    public void CreateOrderItem_CategoryInvalid_ThrowDomainException()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrderItem(1, "Product A", "", 2, 10.0m)
        );

        // Assert
        Assert.Equal("Category not informed", exception.Message);
    }

    [Fact]
    public void CreateOrderItem_QuantityInvalid_ThrowDomainException()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrderItem(1, "Product A", "Category A", 0, 10.0m)
        );

        // Assert
        Assert.Equal("Quantity not informed", exception.Message);
    }

    [Fact]
    public void CreateOrderItem_PriceInvalid_ThrowDomainException()
    {
        // Arrange + Act
        var exception = Assert.Throws<DomainException>(() =>
            OrderEntityFactory.BuildOrderItem(1, "Product A", "Category A", 2, 0m)
        );

        // Assert
        Assert.Equal("Price not informed", exception.Message);
    }
}
