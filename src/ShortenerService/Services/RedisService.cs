using Microsoft.Extensions.Caching.Distributed;

namespace ShortenerService.Services;

public class RedisService(IDistributedCache redisCache)
{
    private readonly IDistributedCache _redisCache = redisCache;

    public async Task SetUrl(string shortCode, string longUrl)
    {
        await _redisCache.SetStringAsync(shortCode, longUrl);
    }

    public async Task<string?> GetUrl(string shortCode)
    {
        return await _redisCache.GetStringAsync(shortCode);
    }

}
