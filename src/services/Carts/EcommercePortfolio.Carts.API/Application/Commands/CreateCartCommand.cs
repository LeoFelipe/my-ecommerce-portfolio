using EcommercePortfolio.Carts.API.Application.Dtos;
using EcommercePortfolio.Carts.Domain.Entities;
using EcommercePortfolio.Core.Messaging;

namespace EcommercePortfolio.Carts.API.Application.Commands;

public record CreateCartCommand(
    Guid ClientId,
    List<CartItemDto> CartItems) : Command
{
    public static explicit operator Cart(CreateCartCommand message)
    {
        return new Cart(message.ClientId, message.CartItems.MapToCartItems());
    }

    //public override bool IsValid()
    //{
    //    ValidationResult = new AddCartValidation().Validate(this);
    //    return ValidationResult.IsValid;
    //}

    //public class AddCartValidation : AbstractValidator<CreateCartCommand>
    //{
    //    public AddCartValidation()
    //    {
    //        RuleFor(x => x.ClientId)
    //            .NotEqual(Guid.Empty)
    //            .WithMessage("Invalid client id");

    //        RuleFor(x => x.CartItems.Count)
    //            .GreaterThan(0)
    //            .WithMessage("The cart needs to have at least 1 item");

    //        RuleFor(x => x.CartItems)
    //            .Must(x => x.All(p => p.IsValid()))
    //            .WithMessage("Invalid cart item");
    //    }
    //}
}
