using Microsoft.Extensions.Configuration;
using ShortenerService.Infrastracture.Context;
using ShortenerService.Infrastracture.Repositories;
using ShortenerService.Services;
using ShortenerService.Shared;
using StackExchange.Redis;

namespace ShortenerService.Bootstraper;

public static class ServiceRegistration
{

    public static void RegisterCommon(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

        builder.Services.AddOpenApi();
    }


    public static void RegisterIoc(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<RedisService>();
        builder.Services.AddScoped<ShortenerContext>();
        builder.Services.AddScoped<UrlDetailsRepository>();
    }


    public static void RegisterRedis(this WebApplicationBuilder builder)
    {
        var redisSettings = builder.Configuration.GetSection("RedisSettings").Get<RedisSettings>();
        if (string.IsNullOrEmpty(redisSettings?.ConnectionString))
            throw new InvalidOperationException("Redis connection string is missing or empty");

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisSettings?.ConnectionString;
        });

    }
}
