using EcommercePortfolio.Domain.Orders.Entities;

namespace EcommercePortfolio.Domain.Orders;

public interface IOrderRepository
{
    Task<Order> GetById(Guid id);
    Task<IEnumerable<Order>> GetByClientId(Guid clientId);
    Task<OrderItem> GetItem(Guid orderId, Guid itemId);
    Task AddAsync(Order order);
    void Update(Order order);
}
