using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Domain.Carts;
using EcommercePortfolio.Domain.Carts.Entities;
using EcommercePortfolio.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EcommercePortfolio.Infra.Orders;

public class CartRepository(MongoDbContext context) : ICartRepository
{
    private readonly MongoDbContext _context = context;

    public async Task<IEnumerable<Cart>> GetByClientId(Guid clientId)
    {
        return await _context.Carts.Where(x => x.GuidId == clientId).ToListAsync();
        //return await _context.Carts.Where(x => x.ClientId == clientId).ToListAsync();
    }

    public async Task<Cart> GetById(string id)
    {
        return await _context.Carts.FindAsync(id);
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

    public void Dispose() => _context.Dispose();
}
