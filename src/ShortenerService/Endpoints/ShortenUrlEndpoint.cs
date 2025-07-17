using ShortenerService.Services;

namespace ShortenerService.Endpoints;

public static class ShortenUrlEndpoint
{
    public static void MapShortenUrlEndpoint(this WebApplication app)
    {
        app.MapGet("/shorten", async (UrlDetailsService urlDetailsService, string url) =>
        {
            var result = await urlDetailsService.ShortenUrl(url);
            return Results.Ok(result.Uri);
        });
    }
}
