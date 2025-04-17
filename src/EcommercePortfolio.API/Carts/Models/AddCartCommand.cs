using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Domain.Carts.Entities;
using FluentValidation;

namespace EcommercePortfolio.API.Carts.Models;

public record AddCartCommand(
    Guid ClientId,
    List<ProductDto> Products) : Command
{
    public static implicit operator Cart(AddCartCommand command)
    {
        return new Cart(command.ClientId, command.Products.ToCartItems());
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

            RuleFor(x => x.Products.Count)
                .GreaterThan(0)
                .WithMessage("The cart needs to have at least 1 item");
        }
    }
}
