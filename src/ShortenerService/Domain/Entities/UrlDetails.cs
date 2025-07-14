using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ShortenerService.Domain.Entities
{
    public class UrlDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; } = null!;
        public string LongUrl { get; private set; } = null!;
        public string ShortCode { get; private set; } = null!;


        private UrlDetails(string longUrl, string shortCode)
        {
            LongUrl = longUrl;
            ShortCode = shortCode;
        }

        public static UrlDetails Create(string longUrl, string shortCode) => new(longUrl, shortCode);
    }
}
