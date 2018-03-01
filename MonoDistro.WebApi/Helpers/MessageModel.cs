using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MonoDistro.WebApi
{
    public class MessageModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Message { get; set; }
    }
}