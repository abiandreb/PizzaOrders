using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using PizzaOrders.Application.Interfaces;

namespace PizzaOrders.Application.Services;

public class RedisService(IDistributedCache cache) : ICacheService
{
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
        };

        var jsonData = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, jsonData, options);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var jsonData = await cache.GetStringAsync(key);
        return jsonData == null ? default : JsonSerializer.Deserialize<T>(jsonData);
    }

    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }
}