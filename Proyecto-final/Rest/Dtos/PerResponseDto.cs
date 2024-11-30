using System.Runtime.Serialization;
using MongoDB.Bson;

namespace Rest.Dtos;

[DataContract]
public class PerResponseDto{
    [DataMember]
    public ObjectId id {get; set;}
    [DataMember]
    public string name {get; set;}
    [DataMember]
    public string arcana {get; set;}
    [DataMember]
    public int level {get; set;}
    [DataMember]
    public Stats stats {get; set;}
    [DataMember]
    public List<string> strength {get; set;}
    [DataMember]
    public List<string> weak {get; set;}
}