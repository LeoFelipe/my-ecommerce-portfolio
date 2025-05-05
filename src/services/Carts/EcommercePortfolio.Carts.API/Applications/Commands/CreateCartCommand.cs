using EcommercePortfolio.Carts.API.Applications.Dtos;
using EcommercePortfolio.Carts.Domain.Entities;
using EcommercePortfolio.Core.Messaging;
using FluentValidation;

namespace EcommercePortfolio.Carts.API.Applications.Commands;

public record CreateCartCommand(
    Guid ClientId,
    List<CartItemDto> CartItems) : Command
{
    public static explicit operator Cart(CreateCartCommand message)
    {
        return new Cart(message.ClientId, message.CartItems.MapToCartItems());
    }

    public override bool IsValid()
    {
        ValidationResult = new CreateCartValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class CreateCartValidation : AbstractValidator<CreateCartCommand>
    {
        public CreateCartValidation()
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
