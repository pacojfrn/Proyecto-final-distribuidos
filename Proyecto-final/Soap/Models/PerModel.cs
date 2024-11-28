using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Soap.Models
{
    public class PerModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("arcana")]
        public string Arcana { get; set; }

        [BsonElement("level")]
        public int Level { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("stats")]
        public Stats Stats { get; set; }

        [BsonElement("strength")]
        public List<string> Strength { get; set; }

        [BsonElement("weak")]
        public List<string> Weak { get; set; }
    }

    public class Stats
    {
        [BsonElement("st")]
        public int St { get; set; }

        [BsonElement("ma")]
        public int Ma { get; set; }

        [BsonElement("en")]
        public int En { get; set; }

        [BsonElement("ag")]
        public int Ag { get; set; }

        [BsonElement("lu")]
        public int Lu { get; set; }
    }
}