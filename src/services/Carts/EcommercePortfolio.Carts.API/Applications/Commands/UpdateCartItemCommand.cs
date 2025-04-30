using EcommercePortfolio.Carts.API.Application.Dtos;
using EcommercePortfolio.Core.Messaging;
using FluentValidation;

namespace EcommercePortfolio.Carts.API.Application.Commands;

public record UpdateCartItemCommand(
    Guid ClientId,
    CartItemDto CartItem) : Command
{
    public override bool IsValid()
    {
        ValidationResult = new UpdateCartItemValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class UpdateCartItemValidation : AbstractValidator<UpdateCartItemCommand>
    {
        public UpdateCartItemValidation()
        {
            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client id");

            RuleFor(x => x.CartItem)
                .Must(x => x.IsValid())
                .WithMessage("Invalid cart item");
        }
    }
}
