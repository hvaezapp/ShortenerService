using Microsoft.AspNetCore.Mvc;
using ShortenerService.Services;

namespace ShortenerService.Endpoints;

public static class RedirectUrlEndpoint
{
    public static void MapRedirectUrlEndpoint(this WebApplication app)
    {
        app.MapGet("/{short_code:required}", async (UrlDetailsService urlDetailsService,
                                                    [FromRoute(Name = "short_code")] string shortCode) =>
        {
            var result = await urlDetailsService.RedirectUrl(shortCode);
            return Results.Redirect(result.Uri.ToString());
        });
    }
}
