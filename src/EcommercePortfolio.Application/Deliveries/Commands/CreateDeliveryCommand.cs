using EcommercePortfolio.Application.Deliveries.Dtos;
using EcommercePortfolio.Core.Messaging;
using FluentValidation;

namespace EcommercePortfolio.Application.Deliveries.Commands;

public record CreateDeliveryCommand(
    Guid OrderId,
    Guid ClientId,
    AddressDto Address) : Command
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

            RuleFor(x => x.Address)
                .Must(x => x.IsValid())
                .WithMessage("Invalid addres");
        }
    }
}