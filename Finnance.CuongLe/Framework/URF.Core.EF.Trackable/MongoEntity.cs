using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace URF.Core.EF.Trackable
{
    public abstract class MongoEntity
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}   