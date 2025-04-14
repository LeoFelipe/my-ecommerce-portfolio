using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Domain.Carts.Entities;

namespace EcommercePortfolio.Domain.Carts;

public interface ICartRepository
{
    Task<IEnumerable<Cart>> GetByClientId(Guid clientId);
    Task<Cart> GetById(string id);
    Task Add(Cart cart);
    void Update(Cart cart);
    void Remove(Cart cart);
}
