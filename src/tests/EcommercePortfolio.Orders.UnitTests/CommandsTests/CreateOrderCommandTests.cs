using EcommercePortfolio.ExternalServices.MyFakePay.Enums;
using EcommercePortfolio.Orders.UnitTests.Factories.Orders;
using MongoDB.Bson;

namespace EcommercePortfolio.Orders.UnitTests.CommandsTests;

public class CreateOrderCommandTests
{
    [Fact]
    public void CreateOrderCommand_ValidCommand_ShouldBeValid()
    {
        // Arrange
        var command = OrderCommandFactory.BuildValidCreateOrderCommand();

        // Act
        var isValid = command.IsValid();

        // Assert
        Assert.True(isValid);
        Assert.Empty(command.ValidationResult.Errors);
    }

    [Fact]
    public void CreateOrderCommand_EmptyCartId_ShouldBeInvalid()
    {
        // Arrange
        var command = OrderCommandFactory.BuildCreateOrderCommand(
            EnumPaymentMethod.CREDIT_CARD,
            "",
            Guid.NewGuid());

        // Act
        var isValid = command.IsValid();

        // Assert
        Assert.False(isValid);
        Assert.Contains(command.ValidationResult.Errors, x => x.ErrorMessage == "Invalid cart id");
    }

    [Fact]
    public void CreateOrderCommand_EmptyClientId_ShouldBeInvalid()
    {
        // Arrange
        var command = OrderCommandFactory.BuildCreateOrderCommand(
            EnumPaymentMethod.PIX,
            new ObjectId().ToString(),
            Guid.Empty);

        // Act
        var isValid = command.IsValid();

        // Assert
        Assert.False(isValid);
        Assert.Contains(command.ValidationResult.Errors, x => x.ErrorMessage == "Invalid client id");
    }

    [Fact]
    public void CreateOrderCommand_InvalidPaymentMethod_ShouldBeInvalid()
    {
        // Arrange
        var invalidPaymentMethod = (EnumPaymentMethod)999;
        var command = OrderCommandFactory.BuildCreateOrderCommand(
            invalidPaymentMethod,
            new ObjectId().ToString(),
            Guid.NewGuid());

        // Act
        var isValid = command.IsValid();

        // Assert
        Assert.False(isValid);
        Assert.Contains(command.ValidationResult.Errors, x => x.ErrorMessage == "Invalid payment method");
    }

    [Fact]
    public void CreateOrderCommand_InvalidAddress_ShouldBeInvalid()
    {
        // Arrange
        var command = OrderCommandFactory.BuildValidCreateOrderCommandWithInvalidAddress();

        // Act
        var isValid = command.IsValid();

        // Assert
        Assert.False(isValid);
        Assert.Contains(command.ValidationResult.Errors, x => x.ErrorMessage == "Invalid addres");
    }
}