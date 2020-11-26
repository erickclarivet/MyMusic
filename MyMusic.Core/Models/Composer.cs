using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyMusic.Core.Models
{
    // This represent our collection for MongoDB (nonsql db)
    public class Composer
    {
        // This define that it's the Id of the collection
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // Details the type of the id
        public ObjectId Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
