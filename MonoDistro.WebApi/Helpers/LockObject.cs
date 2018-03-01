using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MonoDistro.WebApi
{
    public class LockObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Key { get; set; }
        public DateTime Expiration { get; set; }
    }
}