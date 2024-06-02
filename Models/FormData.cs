using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoodFoodProjectMVC.Models
{
    public class FormData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
    }
}
