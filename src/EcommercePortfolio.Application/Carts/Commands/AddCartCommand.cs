using EcommercePortfolio.Application.Carts.Dtos;
using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Domain.Carts.Entities;
using FluentValidation;

namespace EcommercePortfolio.Application.Carts.Commands;

public record AddCartCommand(
    Guid ClientId,
    List<CartItemDto> CartItems) : Command
{
    public static explicit operator Cart(AddCartCommand command)
    {
        return new Cart(command.ClientId, command.CartItems.MapToCartItems());
    }

    public override bool IsValid()
    {
        ValidationResult = new AddCartValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AddCartValidation : AbstractValidator<AddCartCommand>
    {
        public AddCartValidation()
        {
            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client id");

            RuleFor(x => x.CartItems.Count)
                .GreaterThan(0)
                .WithMessage("The cart needs to have at least 1 item");

            RuleFor(x => x.CartItems)
                .Must(x => x.All(p => p.IsValid()))
                .WithMessage("Invalid cart item");
        }
    }
}
