using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ShortenerService.Domain.Entities;
using ShortenerService.Shared;

namespace ShortenerService.Infrastracture.Context;

public sealed class ShortenerContext
{
    public IMongoCollection<UrlDetails> UrlDetails { get; }

    public ShortenerContext(IOptions<DatabaseSettings> databaseSettings)
    {
        var settings = databaseSettings.Value ?? throw new ArgumentNullException(nameof(databaseSettings));

        if (string.IsNullOrWhiteSpace(settings.ConnectionString))
            throw new ArgumentException("ConnectionString is missing.");
        if (string.IsNullOrWhiteSpace(settings.DatabaseName))
            throw new ArgumentException("DatabaseName is missing.");
        if (string.IsNullOrWhiteSpace(settings.CollectionName))
            throw new ArgumentException("CollectionName is missing.");

        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        UrlDetails = database.GetCollection<UrlDetails>(settings.CollectionName);
    }
}