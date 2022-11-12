using System.Text.Json.Serialization;

namespace ctds_webapi.Models;

public class Waiter : Employee
{

    [JsonIgnore]
    public virtual ICollection<Bill> Bills {get;set;}

}

