using EcommercePortfolio.Domain.Orders;
using EcommercePortfolio.Domain.Orders.Entities;

namespace EcommercePortfolio.Infra.Orders;

public class OrderRepository : IOrderRepository
{
    public Task AddAsync(Order order)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Order>> GetByClientId(Guid clientId)
    {
        throw new NotImplementedException();
    }

    public Task<Order> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<OrderItem> GetItem(Guid orderId, Guid itemId)
    {
        throw new NotImplementedException();
    }

    public void Update(Order order)
    {
        throw new NotImplementedException();
    }
}
