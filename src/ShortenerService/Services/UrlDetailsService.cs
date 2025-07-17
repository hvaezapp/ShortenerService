using DispatchR.Requests;
using MongoDB.Driver;
using ShortenerService.Domain.Entities;
using ShortenerService.Infrastracture.Context;
using ShortenerService.Infrastracture.IntegrationEvents;
using ShortenerService.Shared.Utilities;

namespace ShortenerService.Services;

public class UrlDetailsService(ShortenerContext context, RedisService redisService,
                               IConfiguration configuration, IMediator mediator)
{
    private readonly ShortenerContext _context = context;
    private readonly RedisService _redisService = redisService;
    private readonly IConfiguration _configuration = configuration;
    private readonly IMediator _mediator = mediator;


    public async Task<UriBuilder> ShortUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute) is false)
            throw new Exception("The URL query string is required and needs to be well formed");

        var shortCode = AppUtility.GenerateCode(url);

        var newUrlDetails = UrlDetails.Create(url, shortCode);

        // add to mongoDB
        await _context.UrlDetails.InsertOneAsync(newUrlDetails);

        // raise event for add to redis
        await _mediator.Publish(new UrlDetailsChangedEvent(newUrlDetails.ShortCode, newUrlDetails.LongUrl), CancellationToken.None);

        return new UriBuilder(_configuration["BaseUrl"]!) { Path = $"{shortCode}" };
    }


    public async Task<UriBuilder> GetUrl(string shortCode)
    {
        // try to read from redis
        string? longUrl = await _redisService.GetUrl(shortCode);
        if (string.IsNullOrEmpty(longUrl))
        {
            // read from mongodb
            var urlDetails = await _context.UrlDetails.Find(p => p.ShortCode == shortCode).FirstOrDefaultAsync();
            if (urlDetails is null)
                throw new ArgumentNullException(nameof(shortCode), "Shortcode invalid, url not found");

            longUrl = urlDetails.LongUrl;
        }

        if (!Uri.TryCreate(longUrl, UriKind.Absolute, out var validatedUri))
            throw new Exception("Invalid URL");

        // raise event to write on redis for consistency
        await _mediator.Publish(new UrlDetailsChangedEvent(shortCode, longUrl), CancellationToken.None);

        return new UriBuilder(validatedUri);
    }


}

