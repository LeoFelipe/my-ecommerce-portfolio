using EcommercePortfolio.Domain.Carts;

namespace EcommercePortfolio.Application.Carts.Queries;

public interface ICartQueries
{
    Task<GetCartResponse> GetCartById(string id);
    Task<IReadOnlyCollection<GetCartResponse>> GetCartsByClientId(Guid clientId);
}

public class CartQueries(
    ICartRepository cartRepository) : ICartQueries
{
    private readonly ICartRepository _cartRepository = cartRepository;

    public async Task<GetCartResponse> GetCartById(string id)
    {
        var cart = await _cartRepository.GetById(id);
        return (GetCartResponse)cart;
    }

    public async Task<IReadOnlyCollection<GetCartResponse>> GetCartsByClientId(Guid clientId)
    {
        var carts = await _cartRepository.GetCartsByClientId(clientId);
        return carts.ToGetCartResponse();
    }
}
