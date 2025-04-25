using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Domain.Carts.Entities;

namespace EcommercePortfolio.Domain.Carts;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart> GetById(string id);
    Task<Cart> GetByClientId(Guid clientId);
    Task Add(Cart cart);
    void Update(Cart cart);
    void Remove(Cart cart);
}
