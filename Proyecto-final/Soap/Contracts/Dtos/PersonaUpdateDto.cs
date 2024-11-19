using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using Soap.Infrastructure.Entity;


namespace Soap.Contracts.Dtos;

public class PersonaUpdateDto
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
