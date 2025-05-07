using EcommercePortfolio.Carts.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace EcommercePortfolio.Carts.API.Application.Dtos;

public record CartItemDto(
    int ProductId, 
    string Name, 
    string Category, 
    int Quantity, 
    decimal Price)
{
    public static explicit operator CartItem(CartItemDto cartItemDto)
    {
        if (cartItemDto == null)
            return null;

        return new CartItem(
            cartItemDto.ProductId,
            cartItemDto.Name,
            cartItemDto.Category,
            cartItemDto.Quantity,
            cartItemDto.Price);
    }

    public static explicit operator CartItemDto(CartItem cartItem)
    {
        if (cartItem == null)
            return null;

        return new CartItemDto(
            cartItem.ProductId,
            cartItem.ProductName,
            cartItem.Category,
            cartItem.Quantity,
            cartItem.Price);
    }

    [JsonIgnore]
    public ValidationResult ValidationResult { get; set; }

    public bool IsValid()
    {
        ValidationResult = new CartItemDtoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class CartItemDtoValidation : AbstractValidator<CartItemDto>
    {
        public CartItemDtoValidation()
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
    public static List<CartItem> MapToCartItems(this IEnumerable<CartItemDto> cartItemsDto)
        => [.. cartItemsDto.Select(cartItemDto => (CartItem)cartItemDto)];
}