using DispatchR.Extensions;
using DispatchR.Requests.Notification;
using ShortenerService.Events;
using ShortenerService.Handlers;
using ShortenerService.Infrastracture.Context;
using ShortenerService.Services;
using ShortenerService.Shared;
using System.Reflection;

namespace ShortenerService.Bootstraper;

public static class ServiceRegistration
{

    public static void RegisterCommon(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
        builder.Services.AddOpenApi();
    }


    public static void RegisterDispatchR(this WebApplicationBuilder builder)
    {
        builder.Services.AddDispatchR(Assembly.GetExecutingAssembly());
        builder.Services.AddScoped<INotificationHandler<UrlDetailsChangedEvent>, UrlDetailsCreatedEventHandler>();
    }

    public static void RegisterIoc(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UrlDetailsService>();
        builder.Services.AddScoped<RedisService>();
        builder.Services.AddScoped<ShortenerContext>();
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
