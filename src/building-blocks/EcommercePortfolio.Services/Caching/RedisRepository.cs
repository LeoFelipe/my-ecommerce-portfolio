using Microsoft.Extensions.Caching.Hybrid;

namespace EcommercePortfolio.Services.Caching;

public class RedisRepository(HybridCache hybridCache) : IRedisRepository
{
    private readonly HybridCache _hybridCache = hybridCache;

    public async Task<T> GetData<T>(Func<Task<T>> fetchData, string key = "ecommerce-portfolio", TimeSpan? expiration = null)
    {
        ArgumentNullException.ThrowIfNull(fetchData);

        var cacheOptions = expiration.HasValue
            ? new HybridCacheEntryOptions
            {
                Expiration = expiration.Value,
                LocalCacheExpiration = expiration.Value,
            }
            : null;

        return await _hybridCache.GetOrCreateAsync(key, async _ => await fetchData(), cacheOptions);
    }

    public async Task SetData<T>(T value, string key = "ecommerce-portfolio", TimeSpan? expiration = null)
    {
        var cacheOptions = expiration.HasValue
            ? new HybridCacheEntryOptions
            {
                Expiration = expiration.Value,
                LocalCacheExpiration = expiration.Value,
            }
            : null;

        await _hybridCache.SetAsync(key, value, cacheOptions);
    }

    public async Task Remove(string key = "ecommerce-portfolio")
    {
        await _hybridCache.RemoveAsync(key);
    }
}
