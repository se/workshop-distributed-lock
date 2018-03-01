using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MonoDistro.WebApi
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}