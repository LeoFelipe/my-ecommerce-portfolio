using EcommercePortfolio.Core.Messaging;
using FluentValidation;

namespace EcommercePortfolio.Carts.API.Applications.Commands;

public record RemoveCartItemCommand(
    Guid ClientId,
    int ProductId) : Command
{
    public override bool IsValid()
    {
        ValidationResult = new RemoveCartItemValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class RemoveCartItemValidation : AbstractValidator<RemoveCartItemCommand>
    {
        public RemoveCartItemValidation()
        {
            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client id");

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Invalid cart item id");
        }
    }
}
