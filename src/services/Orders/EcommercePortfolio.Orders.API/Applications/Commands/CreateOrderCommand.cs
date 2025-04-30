using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.ExternalServices.MyFakePay.Enums;
using EcommercePortfolio.Orders.API.Applications.Dtos;
using FluentValidation;

namespace EcommercePortfolio.Orders.API.Applications.Commands;

public record CreateOrderCommand(
    string CartId,
    Guid ClientId,
    EnumPaymentMethod PaymentMethod,
    AddressDto Address) : Command
{
    public override bool IsValid()
    {
        ValidationResult = new CreateOrderValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class CreateOrderValidation : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidation()
        {
            RuleFor(x => x.CartId)
                .NotEmpty()
                .WithMessage("Invalid cart id");

            RuleFor(x => x.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid client id");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty()
                .WithMessage("Payment method not informed")
                .IsInEnum()
                .WithMessage("Invalid payment method");

            RuleFor(x => x.Address)
                .Must(x => x.IsValid())
                .WithMessage("Invalid addres");
        }
    }
}