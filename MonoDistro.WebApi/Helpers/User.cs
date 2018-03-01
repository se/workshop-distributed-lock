using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MonoDistro.WebApi
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
    }
}