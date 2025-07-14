using Microsoft.Extensions.Configuration;
using Scalar.AspNetCore;
using ShortenerService.Infrastracture.Context;
using ShortenerService.Infrastracture.Repositories;
using ShortenerService.Models;
using ShortenerService.Shared;
using ShortenerService.Shared.Utilities;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddScoped<ShortenerContext>();
builder.Services.AddScoped<UrlDetailsRepository>();

builder.Services.AddOpenApi();


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
    {
        return Results.BadRequest("The URL query string is required and needs to be well formed");
    }

    var shortCode = AppUtility.GenerateCode(longUrl);

    var newUrlDetails = UrlDetails.Create(longUrl, shortCode);

    await urlDetailsRepository.Create(newUrlDetails);


    var resultBuilder = new UriBuilder(configuration["BaseUrl"]!) { Path = $"{shortCode}" };
    return Results.Ok(resultBuilder.Uri);

});



app.UseHttpsRedirection();
app.Run();
