using EcommercePortfolio.Core.Messaging;
using FluentValidation;

namespace EcommercePortfolio.Deliveries.API.Applications.Commands;

public record CreateDeliveryCommand(
    Guid OrderId,
    Guid ClientId) : Command
{
    public override bool IsValid()
    {
        ValidationResult = new CreateDeliveryValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class CreateDeliveryValidation : AbstractValidator<CreateDeliveryCommand>
    {
        public CreateDeliveryValidation()
        {
            RuleFor(x => x.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid order id");

            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client id");
        }
    }
}
