using System.Text.Json.Serialization;

namespace ctds_webapi.Models;

public class Table
{
    public Guid TableId {get;set;}
    public string Name {get;set;}
    public bool Reserved {get;set;}
    public int Stalls {get;set;}

    [JsonIgnore]
    public virtual ICollection<Bill> Bills {get;set;}

}