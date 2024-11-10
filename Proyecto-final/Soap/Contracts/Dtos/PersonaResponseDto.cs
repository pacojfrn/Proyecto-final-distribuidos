using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Soap.Contracts.Dtos;
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

public class PersonaResponseDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } 
    
    [BsonElement("arcana")]
    public string Arcana { get; set; }

    [BsonElement("weak")]
    public List<string> Weak { get; set; }

    [BsonElement("stats")]
    public List<Stats> Stats { get; set; }

    [BsonElement("strength")]
    public List<string> Strength { get; set; }

    [BsonElement("level")]
    public int Level { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }
}
