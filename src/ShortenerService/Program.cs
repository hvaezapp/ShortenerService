using Scalar.AspNetCore;
using ShortenerService.Bootstraper;
using ShortenerService.Endpoints;

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

app.MapShortenUrlEndpoint();
app.MapRedirectUrlEndpoint();

app.Run();

