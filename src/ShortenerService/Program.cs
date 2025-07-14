using Scalar.AspNetCore;
using ShortenerService.Infrastracture.Context;
using ShortenerService.Infrastracture.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<UrlDetailsRepository>();
builder.Services.AddScoped<ShortenerContext>();

builder.Services.AddOpenApi();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseHttpsRedirection();
app.Run();
