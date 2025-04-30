namespace EcommercePortfolio.Services.Caching;

public interface IRedisRepository
{
    Task<T> GetData<T>(Func<Task<T>> fetchData, string key = "ecommerce-portfolio", TimeSpan? expiration = null);
    Task SetData<T>(T value, string key = "ecommerce-portfolio", TimeSpan? expiration = null);
}
