using FluentAssertions;
using Npgsql;

namespace EcommercePortfolio.IntegrationTests.AssertsHelper;

public static class PostgresAssertHelper
{
    public static async Task AssertOrderExistsAsync(string connectionString, Guid clientId)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM \"Orders\" WHERE \"client_id\" = @clientId", conn);
        cmd.Parameters.AddWithValue("clientId", clientId);
        var result = await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException("Query result is null.");
        var count = (long)result;
        count.Should().Be(1);
    }

    public static async Task AssertDeliveryExistsAsync(string connectionString, Guid clientId)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM \"Deliveries\" WHERE \"client_id\" = @clientId", conn);
        cmd.Parameters.AddWithValue("clientId", clientId);
        var result = await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException("Query result is null.");
        var count = (long)result;
        count.Should().Be(1);
    }
}