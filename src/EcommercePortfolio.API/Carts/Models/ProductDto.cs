using EcommercePortfolio.Domain.Carts.Entities;

namespace EcommercePortfolio.API.Carts.Models;

public record ProductDto(
    int ProductId, 
    string Name, 
    string Category, 
    int Quantity, 
    decimal Price)
{
    public static implicit operator CartItem(ProductDto productDto)
    {
        return new CartItem(
            productDto.ProductId,
            productDto.Name,
            productDto.Category,
            productDto.Quantity,
            productDto.Price);
    }
}

public static class ProductDtoExtensions
{
    public static CartItem ToCartItem(this ProductDto productDto) 
        => (CartItem)productDto;

    public static List<CartItem> ToCartItems(this IEnumerable<ProductDto> productDtos)
        => [.. productDtos.Select(productDto => productDto.ToCartItem())];
}