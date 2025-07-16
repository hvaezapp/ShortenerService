using Microsoft.Extensions.Caching.Distributed;
using ShortenerService.Domain.Entities;
using System.Text.Json;

namespace ShortenerService.Services
{
    public class RedisService(IDistributedCache redisCache)
    {
        private readonly IDistributedCache _redisCache = redisCache;

        public async Task SetUrlDeatils(string shortCode, string longUrl)
        {
            await _redisCache.SetStringAsync(shortCode, longUrl);
        }

        public async Task<UrlDetails> GetUrlDeatils(string shortCode)
        {
            return null;
        }

    }
}
