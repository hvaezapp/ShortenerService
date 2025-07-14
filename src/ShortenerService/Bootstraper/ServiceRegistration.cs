using ShortenerService.Infrastracture.Context;
using ShortenerService.Infrastracture.Repositories;
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
        builder.Services.AddScoped<ShortenerContext>();
        builder.Services.AddScoped<UrlDetailsRepository>();
    }


    public static void RegisterRedis(this WebApplicationBuilder builder)
    {
        var RedisConnectionString = builder.Configuration.GetConnectionString("RedisSettings");
        if (string.IsNullOrEmpty(RedisConnectionString))
            throw new InvalidOperationException("This Redis connection string is missing or empty");
        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(RedisConnectionString));
    }
}
