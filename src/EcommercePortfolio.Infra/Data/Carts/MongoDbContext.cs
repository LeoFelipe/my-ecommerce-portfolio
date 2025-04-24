using EcommercePortfolio.Core.Data;
using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Domain.Carts.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

namespace EcommercePortfolio.Infra.Data.Contexts;

public class MongoDbContext(DbContextOptions<MongoDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Cart> Carts { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Event>();

        modelBuilder.Entity<Cart>(c =>
        {
            c.ToCollection("cart");
            c.HasKey(c => c.Id);
            c.Property(p => p.Id)
                .HasConversion(id => ObjectId.Parse(id), oid => oid.ToString());
        });

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit() => await base.SaveChangesAsync() > 0;
}