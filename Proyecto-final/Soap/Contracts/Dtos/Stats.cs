using System.Runtime.Serialization;

namespace Soap.Dtos;

[DataContract]
public class Stats
{
    [DataMember]
    public int St { get; set; }

    [DataMember]
    public int Ma { get; set; }

    [DataMember]
    public int En { get; set; }

    [DataMember]
    public int Ag { get; set; }

    [DataMember]
    public int Lu { get; set; }
}
