using EcommercePortfolio.ExternalServices.MyFakePay.Enums;
using EcommercePortfolio.Orders.API.Application.Commands;
using EcommercePortfolio.Orders.API.Application.Dtos;
using MongoDB.Bson;

namespace EcommercePortfolio.Orders.UnitTests.CommandsTests;

public class CreateOrderCommandTests
{
    private static AddressDto CreateValidAddress() =>
        new ("49000-000", "Sergipe", "Aracaju", "Av. Teste", 123);

    [Fact]
    public void CreateOrderCommand_ValidCommand_ShouldBeValid()
    {
        // Arrange
        var command = new CreateOrderCommand(
            new ObjectId().ToString(),
            Guid.NewGuid(),
            EnumPaymentMethod.PIX,
            CreateValidAddress()
        );

        // Act
        var isValid = command.IsValid();

        // Assert
        Assert.True(isValid);
        Assert.Empty(command.ValidationResult.Errors);
    }

    [Fact]
    public void CreateOrderCommand_EmptyCartId_ShouldBeInvalid()
    {
        var command = new CreateOrderCommand(
            "",
            Guid.NewGuid(),
            EnumPaymentMethod.CREDIT_CARD,
            CreateValidAddress()
        );

        var isValid = command.IsValid();

        Assert.False(isValid);
        Assert.Contains(command.ValidationResult.Errors, x => x.ErrorMessage == "Invalid cart id");
    }

    [Fact]
    public void CreateOrderCommand_EmptyClientId_ShouldBeInvalid()
    {
        var command = new CreateOrderCommand(
            new ObjectId().ToString(),
            Guid.Empty,
            EnumPaymentMethod.PIX,
            CreateValidAddress()
        );

        var isValid = command.IsValid();

        Assert.False(isValid);
        Assert.Contains(command.ValidationResult.Errors, x => x.ErrorMessage == "Invalid client id");
    }

    [Fact]
    public void CreateOrderCommand_InvalidPaymentMethod_ShouldBeInvalid()
    {
        var invalidPaymentMethod = (EnumPaymentMethod)999;

        var command = new CreateOrderCommand(
            new ObjectId().ToString(),
            Guid.NewGuid(),
            invalidPaymentMethod,
            CreateValidAddress()
        );

        var isValid = command.IsValid();

        Assert.False(isValid);
        Assert.Contains(command.ValidationResult.Errors, x => x.ErrorMessage == "Invalid payment method");
    }

    [Fact]
    public void CreateOrderCommand_InvalidAddress_ShouldBeInvalid()
    {
        var invalidAddress = new AddressDto("", "", "", "", 0);

        var command = new CreateOrderCommand(
            new ObjectId().ToString(),
            Guid.NewGuid(),
            EnumPaymentMethod.PIX,
            invalidAddress
        );

        var isValid = command.IsValid();

        Assert.False(isValid);
        Assert.Contains(command.ValidationResult.Errors, x => x.ErrorMessage == "Invalid addres");
    }
}