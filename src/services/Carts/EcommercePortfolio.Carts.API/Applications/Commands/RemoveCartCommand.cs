using EcommercePortfolio.Core.Messaging;
using FluentValidation;

namespace EcommercePortfolio.Carts.API.Applications.Commands;

public record RemoveCartCommand(
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
            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client id");
        }
    }
}
