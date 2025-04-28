using EcommercePortfolio.Core.Messaging;
using FluentValidation;

namespace EcommercePortfolio.Carts.API.Application.Commands;

public record RemoveCartItemCommand(
    string Id,
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
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Invalid cart id");

            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client id");

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Invalid cart item id");
        }
    }
}
