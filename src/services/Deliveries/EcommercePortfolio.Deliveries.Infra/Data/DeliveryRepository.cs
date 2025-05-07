using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Deliveries.Domain;
using EcommercePortfolio.Deliveries.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Deliveries.Infra.Data;

public class DeliveryRepository(DeliveryPostgresDbContext context) : IDeliveryRepository
{
    private readonly DeliveryPostgresDbContext _context = context;
    public IUnitOfWork UnitOfWork => _context;

    private bool _disposed;

    public async Task<Delivery> GetById(Guid id)
    {
        return await _context.Delivery.FindAsync(id);
    }

    public async Task<Delivery> GetByOrderId(Guid orderId)
    {
        return await _context.Delivery.SingleOrDefaultAsync(x => x.OrderId == orderId);
    }
    
    public async Task AddAsync(Delivery delivery)
    {
        await _context.Delivery.AddAsync(delivery);
    }

    public void Update(Delivery delivery)
    {
        _context.Delivery.Update(delivery);
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
