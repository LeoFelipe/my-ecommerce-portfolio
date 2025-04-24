using EcommercePortfolio.Application.Orders.Dtos;
using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Domain.Payments.Enums;
using FluentValidation;

namespace EcommercePortfolio.Application.Orders.Commands;

public record AddOrderCommand(
    string CartId,
    Guid ClientId,
    EnumPaymentMethod PaymentMethod,
    AddressDto Address) : Command
{
    public override bool IsValid()
    {
        ValidationResult = new AddOrderValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AddOrderValidation : AbstractValidator<AddOrderCommand>
    {
        public AddOrderValidation()
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