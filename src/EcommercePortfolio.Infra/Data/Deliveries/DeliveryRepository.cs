using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Domain.Deliveries;
using EcommercePortfolio.Domain.Deliveries.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Infra.Data.Deliveries;

public class DeliveryRepository(DeliveryPostgresDbContext context) : IDeliveryRepository
{
    private readonly DeliveryPostgresDbContext _context = context;
    public IUnitOfWork UnitOfWork => _context;

    private bool _disposed;

    public async Task<Delivery> GetById(Guid id)
    {
        return await _context.Delivery.FindAsync(id);
    }
    public async Task<IEnumerable<Delivery>> GetByClientId(Guid clientId)
    {
        return await _context.Delivery.Where(x => x.ClientId == clientId).ToListAsync();
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
