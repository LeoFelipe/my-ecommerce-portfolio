using EcommercePortfolio.Core.Messaging;
using FluentValidation;

namespace EcommercePortfolio.API.Carts.Models;

public record RemoveCartCommand(
    string Id,
    Guid ClientId) : Command
{
    public override bool IsValid()
    {
        ValidationResult = new RemoveCartValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class RemoveCartValidation : AbstractValidator<RemoveCartCommand>
    {
        public RemoveCartValidation()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Invalid cart id");

            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client id");
        }
    }
}
