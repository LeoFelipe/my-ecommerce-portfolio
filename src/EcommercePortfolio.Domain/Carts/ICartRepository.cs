using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Domain.Carts.Entities;
using MongoDB.Bson;

namespace EcommercePortfolio.Domain.Carts;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart> GetById(string id);
    Task<IEnumerable<Cart>> GetByClientId(Guid clientId);
    Task Add(Cart cart);
    void Update(Cart cart);
    void Remove(Cart cart);
}
