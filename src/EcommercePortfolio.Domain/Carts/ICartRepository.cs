using EcommercePortfolio.Domain.Carts.Entities;

namespace EcommercePortfolio.Domain.Carts;

public interface ICartRepository
{
    Task<Cart> GetById(Guid id);
    Task<IEnumerable<Cart>> GetByClientId(Guid clientId);
    void Add(Cart order);
    void Update(Cart order);
    void Remove(Cart order);
}
