using EcommercePortfolio.Application.Carts.Dtos;
using EcommercePortfolio.Core.Messaging;
using FluentValidation;

namespace EcommercePortfolio.Application.Carts.Commands;

public record UpdateCartCommand(
    string Id,
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
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Invalid cart id");

            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client id");

            RuleFor(x => x.Products.Count)
                .GreaterThan(0)
                .WithMessage("The cart needs to have at least 1 item");
        }
    }
}
