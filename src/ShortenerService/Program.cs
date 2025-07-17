using DispatchR.Requests.Notification;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using ShortenerService.Bootstraper;
using ShortenerService.Infrastracture.Context;
using ShortenerService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterCommon();
builder.RegisterRedis();
builder.RegisterIoc();
builder.RegisterDispatchR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.MapGet("/shorten", async (UrlDetailsService urlDetailsService, string url) =>
{
    var result = await urlDetailsService.ShortUrl(url);
    return Results.Ok(result.Uri);

});


app.MapGet("/{short_code:required}", async (UrlDetailsService urlDetailsService,
                                           [FromRoute(Name = "short_code")] string shortCode) =>
{
    var result = await urlDetailsService.GetUrl(shortCode);
    return Results.Redirect(result.Uri.ToString());

});

app.Run();

