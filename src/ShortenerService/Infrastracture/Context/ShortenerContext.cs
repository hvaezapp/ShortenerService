using MongoDB.Driver;

namespace ShortenerService.Infrastracture.Context;

public class ShortenerContext
{
    public IMongoCollection<UrlDetails> UrlDetails { get; }

    public ShortenerContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        UrlDetails = database.GetCollection<UrlDetails>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
    }
}
