using EcommercePortfolio.Domain.Carts.Entities;
using FluentValidation;
using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace EcommercePortfolio.Application.Carts.Dtos;

public record CartItemDto(
    int ProductId, 
    string Name, 
    string Category, 
    int Quantity, 
    decimal Price)
{
    public static implicit operator CartItem(CartItemDto cartItemDto)
    {
        return new CartItem(
            cartItemDto.ProductId,
            cartItemDto.Name,
            cartItemDto.Category,
            cartItemDto.Quantity,
            cartItemDto.Price);
    }

    public static implicit operator CartItemDto(CartItem cartItem)
    {
        return new CartItemDto(
            cartItem.ProductId,
            cartItem.ProductName,
            cartItem.Category,
            cartItem.Quantity,
            cartItem.Price);
    }

    [JsonIgnore]
    public ValidationResult? ValidationResult { get; set; }

    public bool IsValid()
    {
        ValidationResult = new AddCartItemValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AddCartItemValidation : AbstractValidator<CartItemDto>
    {
        public AddCartItemValidation()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Invalid id");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Invalid name");

            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Invalid category");

            RuleFor(x => x.Quantity)
                .NotEmpty()
                .WithMessage("Invalid quantity");

            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Invalid price");
        }
    }
}

public static class CartItemDtoExtensions
{
    public static List<CartItem> ToCartItems(this IEnumerable<CartItemDto> cartItemsDto)
        => [.. cartItemsDto.Select(cartItemDto => cartItemDto)];
}