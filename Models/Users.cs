using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoodFoodProjectMVC.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
