using System.Text.Json.Serialization;

namespace ctds_webapi.Models;
public class Customer : Person
{
    public string Address {get;set;}
    public long Cellphone {get;set;}

    [JsonIgnore]
    public virtual ICollection<Bill> Bills {get;set;}
}