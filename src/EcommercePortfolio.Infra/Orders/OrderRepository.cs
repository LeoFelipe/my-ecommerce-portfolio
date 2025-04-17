using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Domain.Orders;
using EcommercePortfolio.Domain.Orders.Entities;
using EcommercePortfolio.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Infra.Orders;

public class OrderRepository(PostgresDbContext context) : IOrderRepository
{
    private readonly PostgresDbContext _context = context;
    public IUnitOfWork UnitOfWork => _context;

    public async Task<Order> GetById(Guid id)
    {
        return await _context.Orders.FindAsync(id);
    }
    public async Task<IEnumerable<Order>> GetByClientId(Guid clientId)
    {
        return await _context.Orders.Where(x => x.ClientId == clientId).ToListAsync();
    }
    public async Task<OrderItem> GetItem(Guid orderId, Guid itemId)
    {
        return await _context.OrderItems.FirstOrDefaultAsync(x => x.Id == itemId && x.OrderId == orderId);
    }
    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }
    public void Update(Order order)
    {
        _context.Orders.Update(order);
    }
}