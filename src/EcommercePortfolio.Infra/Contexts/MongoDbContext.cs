using EcommercePortfolio.Domain.Carts.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace EcommercePortfolio.Infra.Contexts;

public class MongoDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Cart> Carts { get; init; }

    public static MongoDbContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<MongoDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cart>().ToCollection("cart");
    }
}