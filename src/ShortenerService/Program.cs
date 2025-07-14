using Scalar.AspNetCore;
using ShortenerService.Bootstraper;
using ShortenerService.Domain.Entities;
using ShortenerService.Infrastracture.Repositories;
using ShortenerService.Shared.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterCommon();
builder.RegisterIoc();
builder.RegisterRedis();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}



app.MapGet("/shorten", async (UrlDetailsRepository urlDetailsRepository,
                              IConfiguration configuration,
                              string longUrl) =>
{
    if (string.IsNullOrWhiteSpace(longUrl) || Uri.IsWellFormedUriString(longUrl, UriKind.Absolute) is false)
        return Results.BadRequest("The URL query string is required and needs to be well formed");

    var shortCode = AppUtility.GenerateCode(longUrl);

    // save in redis cash

    var newUrlDetails = UrlDetails.Create(longUrl, shortCode);

    await urlDetailsRepository.Create(newUrlDetails);


    var resultBuilder = new UriBuilder(configuration["BaseUrl"]!) { Path = $"{shortCode}" };
    return Results.Ok(resultBuilder.Uri);

});




app.Run();
