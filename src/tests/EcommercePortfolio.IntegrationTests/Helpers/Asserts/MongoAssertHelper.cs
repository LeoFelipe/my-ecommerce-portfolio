using EcommercePortfolio.Carts.Domain.Entities;
using FluentAssertions;
using MongoDB.Driver;

namespace EcommercePortfolio.IntegrationTests.Helpers.Asserts;

public static class MongoAssertHelper
{
    public static async Task AssertCartDoesNotExistAsync(string connectionString, Guid clientId)
    {
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("EcommercePortfolioCart");
        var cartsCollection = database.GetCollection<Cart>("cart");

        var cartInDabase = await cartsCollection.Find(c => c.ClientId == clientId).FirstOrDefaultAsync();
        cartInDabase.Should().BeNull();
    }
}