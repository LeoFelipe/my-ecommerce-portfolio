namespace EcommercePortfolio.Deliveries.API.Dtos;

public record AddressDto(
    string ZipCode,
    string State,
    string City,
    string StreetAddress,
    int NumberAddress)
{
    //[JsonIgnore]
    //public ValidationResult? ValidationResult { get; set; }

    //public bool IsValid()
    //{
    //    ValidationResult = new AddressDtoValidation().Validate(this);
    //    return ValidationResult.IsValid;
    //}

    //public class AddressDtoValidation : AbstractValidator<AddressDto>
    //{
    //    public AddressDtoValidation()
    //    {
    //        RuleFor(x => x.ZipCode)
    //            .NotEmpty()
    //            .WithMessage("Invalid zip code");

    //        RuleFor(x => x.State)
    //            .NotEmpty()
    //            .WithMessage("Invalid state");

    //        RuleFor(x => x.City)
    //            .NotEmpty()
    //            .WithMessage("Invalid city");

    //        RuleFor(x => x.StreetAddress)
    //            .NotEmpty()
    //            .WithMessage("Invalid street address");

    //        RuleFor(x => x.NumberAddress)
    //            .NotEmpty()
    //            .WithMessage("Invalid building number");
    //    }
    //}
}