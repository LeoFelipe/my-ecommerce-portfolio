using EcommercePortfolio.Domain.Caching;
using Microsoft.Extensions.Caching.Hybrid;

namespace EcommercePortfolio.Infra.Data.Caching;

public class RedisRepository(HybridCache hybridCache) : IRedisRepository
{
    private readonly HybridCache _hybridCache = hybridCache;

    public async Task<T> GetData<T>(Func<Task<T>> GetData, string key = "ecommerce-portfolio", TimeSpan? expiration = null)
    {
        if (expiration.HasValue)
        {
            return await _hybridCache.GetOrCreateAsync(key,
                async cancellationToken => await GetData(),
                new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromSeconds(10),
                    LocalCacheExpiration = TimeSpan.FromSeconds(10),
                });
        }

        return await _hybridCache.GetOrCreateAsync(key, async cancellationToken => await GetData());
    }

    public async Task SetData<T>(T value, string key = "ecommerce-portfolio", TimeSpan? expiration = null)
    {
        if (expiration.HasValue)
        {
            await _hybridCache.SetAsync(key,
            value,
            new HybridCacheEntryOptions
            {
                Expiration = expiration.Value,
                LocalCacheExpiration = expiration.Value,
            });
            return;
        }

        await _hybridCache.SetAsync(key, value);
    }

    public async Task Remove(string key = "ecommerce-portfolio")
    {
        await _hybridCache.RemoveAsync(key);
    }
}
