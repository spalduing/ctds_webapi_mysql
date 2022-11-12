using System.Text.Json.Serialization;

namespace ctds_webapi.Models;

public class Manager : Employee
{
    [JsonIgnore]
    public virtual ICollection<Detail_Bill> Detail_Bills {get;set;}
}