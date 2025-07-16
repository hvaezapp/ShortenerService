using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Scalar.AspNetCore;
using ShortenerService.Bootstraper;
using ShortenerService.Domain.Entities;
using ShortenerService.Infrastracture.Repositories;
using ShortenerService.Shared.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterCommon();
builder.RegisterRedis();
builder.RegisterIoc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.MapGet("/shorten", async (UrlDetailsRepository urlDetailsRepository,
                              IConfiguration configuration,
                              string url) =>
{
    if (string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute) is false)
        return Results.BadRequest("The URL query string is required and needs to be well formed");

    var shortCode = AppUtility.GenerateCode(url);

    // save in redis cash

    var newUrlDetailsItem = UrlDetails.Create(url, shortCode);

    // addd to mongo
    await urlDetailsRepository.Create(newUrlDetailsItem);


    var resultBuilder = new UriBuilder(configuration["BaseUrl"]!) { Path = $"{shortCode}" };
    return Results.Ok(resultBuilder.Uri);

});


app.MapGet("/{short_code:required}", async (UrlDetailsRepository urlDetailsRepository,
                                           [FromRoute(Name = "short_code")] string shortCode) =>
{
        var urlDetailsItem = await urlDetailsRepository.GetByShortCode(shortCode);
        if (urlDetailsItem is null)
            return Results.BadRequest("Shortcode invalid, url not found");

        if (Uri.TryCreate(urlDetailsItem.LongUrl, UriKind.Absolute, out var validatedUri))
        {
            var redirectBuilder = new UriBuilder(validatedUri);
            return Results.Redirect(redirectBuilder.Uri.ToString());
        }
        else
            return Results.BadRequest("Invalid URL");
});


app.MapGet("getAll" , async (UrlDetailsRepository urlDetailsRepository) =>
{
    return Results.Ok( await urlDetailsRepository.GetAll());
});


app.Run();
