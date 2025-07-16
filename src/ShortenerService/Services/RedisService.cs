using Microsoft.Extensions.Caching.Distributed;
using ShortenerService.Domain.Entities;

namespace ShortenerService.Services
{
    public class RedisService
    {
        private readonly IDistributedCache _redisCache;

        public RedisService(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task UpdateUserActionHistory(string shortCode, UrlDetails urlDetails)
        {
            await _redisCache.SetStringAsync(shortCode, null);
        }

    }
}
