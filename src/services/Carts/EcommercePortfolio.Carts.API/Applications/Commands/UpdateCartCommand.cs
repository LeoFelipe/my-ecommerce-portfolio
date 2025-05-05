using EcommercePortfolio.Carts.API.Applications.Dtos;
using EcommercePortfolio.Core.Messaging;
using FluentValidation;

namespace EcommercePortfolio.Carts.API.Applications.Commands;

public record UpdateCartCommand(
    Guid ClientId,
    List<CartItemDto> Products) : Command
{
    public override bool IsValid()
    {
        ValidationResult = new UpdateCartValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class UpdateCartValidation : AbstractValidator<UpdateCartCommand>
    {
        public UpdateCartValidation()
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
