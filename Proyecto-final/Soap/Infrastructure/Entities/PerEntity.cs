using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Soap.Infrastructure.Entities
{
    public class PerEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId id { get; set; }

        [BsonElement("arcana")]
        public string arcana { get; set; }

        [BsonElement("level")]
        public int level { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("stats")]
        public Stats stats { get; set; }

        [BsonElement("strength")]
        public List<string> strength { get; set; }

        [BsonElement("weak")]
        public List<string> weak { get; set; }
    }

    public class Stats
    {
        [BsonElement("St")]
        public int St { get; set; }

        [BsonElement("Ma")]
        public int Ma { get; set; }

        [BsonElement("En")]
        public int En { get; set; }

        [BsonElement("Ag")]
        public int Ag { get; set; }

        [BsonElement("Lu")]
        public int Lu { get; set; }
    }
}