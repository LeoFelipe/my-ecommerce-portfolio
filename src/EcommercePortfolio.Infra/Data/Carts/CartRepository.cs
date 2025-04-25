using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Domain.Carts;
using EcommercePortfolio.Domain.Carts.Entities;
using EcommercePortfolio.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Infra.Data.Carts;

public class CartRepository(MongoDbContext context) : ICartRepository
{
    private readonly MongoDbContext _context = context;
    public IUnitOfWork UnitOfWork => _context;

    private bool _disposed;

    public async Task<Cart> GetById(string id)
    {
        return await _context.Carts.FindAsync(id);
    }

    public async Task<Cart> GetByClientId(Guid clientId)
    {
        return await _context.Carts.SingleOrDefaultAsync(x => x.ClientId == clientId);
    }

    public async Task Add(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
    }

    public void Update(Cart cart)
    {
        _context.Carts.Update(cart);
    }

    public void Remove(Cart cart)
    {
        _context.Carts.Remove(cart);
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
