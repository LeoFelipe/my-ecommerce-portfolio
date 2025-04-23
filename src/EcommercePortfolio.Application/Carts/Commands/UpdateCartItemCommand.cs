using EcommercePortfolio.Application.Carts.Dtos;
using EcommercePortfolio.Core.Messaging;
using FluentValidation;

namespace EcommercePortfolio.Application.Carts.Commands;

public record UpdateCartItemCommand(
    string Id,
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
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Invalid cart id");

            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client id");

            RuleFor(x => x.CartItem)
                .Must(x => x.IsValid())
                .WithMessage("Invalid cart item");
        }
    }
}
