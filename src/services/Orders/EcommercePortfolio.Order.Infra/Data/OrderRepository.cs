using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Orders.Domain;
using EcommercePortfolio.Orders.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Orders.Infra.Data;

public class OrderRepository(OrderPostgresDbContext context) : IOrderRepository
{
    private readonly OrderPostgresDbContext _context = context;
    public IUnitOfWork UnitOfWork => _context;

    private bool _disposed;

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

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}