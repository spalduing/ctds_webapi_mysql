using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ctds_webapi.Models;
public class CreateTableDTO
{
    [Required]
    public string Name {get;set;}
    [Required]
    public bool Reserved {get;set;}
    [Required]
    public int Stalls {get;set;}
    public DateTime CreatedAt {get;set;}

}

public class UpdateTableDTO : CreateTableDTO
{

}

public class TableDTO : CreateTableDTO
{
    public Guid TableId {get;set;}

    [JsonIgnore]
    public ICollection<BillDTO> Bills {get;set;}

}