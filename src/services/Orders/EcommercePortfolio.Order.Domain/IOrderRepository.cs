using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Orders.Domain.Entities;

namespace EcommercePortfolio.Orders.Domain;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> GetById(Guid id);
    Task<IEnumerable<Order>> GetByClientId(Guid clientId);
    Task<OrderItem> GetItem(Guid orderId, Guid itemId);
    Task AddAsync(Order order);
    void Update(Order order);
}
