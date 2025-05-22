using EcommercePortfolio.ApiGateways.MyFakePay.Enums;
using EcommercePortfolio.Orders.UnitTests.Factories.Orders;
using FluentAssertions;
using MongoDB.Bson;

namespace EcommercePortfolio.Orders.UnitTests.CommandsTests;

public class CreateOrderCommandTests
{
    [Fact]
    public void CreateOrderCommand_Validate_ShouldBeValidWhenAllPropertiesValid()
    {
        // Arrange
        var command = OrderCommandFactory.BuildValidCreateOrderCommand();

        // Act
        var isValid = command.IsValid();

        // Assert
        isValid.Should().BeTrue();
        command.ValidationResult.Errors.Should().BeEmpty();
    }

    [Fact]
    public void CreateOrderCommand_Validate_ShouldBeInvalidWhenCartIdEmpty()
    {
        // Arrange
        var command = OrderCommandFactory.BuildCreateOrderCommand(
            EnumPaymentMethod.CREDIT_CARD,
            "",
            Guid.NewGuid());

        // Act
        var isValid = command.IsValid();

        // Assert
        isValid.Should().BeFalse();
        command.ValidationResult.Errors.Should().Contain(x => x.ErrorMessage == "Invalid cart id");
    }

    [Fact]
    public void CreateOrderCommand_Validate_ShouldBeInvalidWhenClientIdEmpty()
    {
        // Arrange
        var command = OrderCommandFactory.BuildCreateOrderCommand(
            EnumPaymentMethod.PIX,
            new ObjectId().ToString(),
            Guid.Empty);

        // Act
        var isValid = command.IsValid();

        // Assert
        isValid.Should().BeFalse();
        command.ValidationResult.Errors.Should().Contain(x => x.ErrorMessage == "Invalid client id");
    }

    [Fact]
    public void CreateOrderCommand_Validate_ShouldBeInvalidWhenPaymentMethodInvalid()
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
        isValid.Should().BeFalse();
        command.ValidationResult.Errors.Should().Contain(x => x.ErrorMessage == "Invalid payment method");
    }

    [Fact]
    public void CreateOrderCommand_Validate_ShouldBeInvalidWhenAddressInvalid()
    {
        // Arrange
        var command = OrderCommandFactory.BuildValidCreateOrderCommandWithInvalidAddress();

        // Act
        var isValid = command.IsValid();

        // Assert
        isValid.Should().BeFalse();
        command.ValidationResult.Errors.Should().Contain(x => x.ErrorMessage == "Invalid addres");
    }
}