using DispatchR.Requests;
using MongoDB.Driver;
using ShortenerService.Domain.Entities;
using ShortenerService.Events;
using ShortenerService.Infrastracture.Context;
using ShortenerService.Shared.Utilities;

namespace ShortenerService.Services;

public class UrlDetailsService(ShortenerContext context , RedisService redisService , 
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
        await _mediator.Publish(new UrlDetailsCreatedEvent(newUrlDetails.ShortCode, newUrlDetails.LongUrl), CancellationToken.None);

        return new UriBuilder(_configuration["BaseUrl"]!) { Path = $"{shortCode}" };
    }


    public async Task<UriBuilder> GetUrl(string shortCode)
    {
        UrlDetails urlDetails = null;

        // try to read from redis
        urlDetails = _redisService.GetUrlDeatils(shortCode);

        var urlDetails = await _context.UrlDetails.Find(p => p.ShortCode == shortCode).FirstOrDefaultAsync();
        if (urlDetails is null)
            throw new ArgumentNullException(nameof(shortCode), "Shortcode invalid, url not found");

        if (!Uri.TryCreate(urlDetails.LongUrl, UriKind.Absolute, out var validatedUri))
            throw new Exception("Invalid URL");

        return new UriBuilder(validatedUri);
    }


}

