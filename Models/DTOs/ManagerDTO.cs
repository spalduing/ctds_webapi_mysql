using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ctds_webapi.Models;
public class CreateManagerDTO
{
    [Required]
    public string Name {get;set;}

    [Required]
    public string LastName {get;set;}

    [Required]
    public int Age {get;set;}

    [Required]
    public Seniority Seniority {get;set;}

}

public class ManagerDTO : CreateManagerDTO
{
    public Guid Id {get;set;}

    [JsonIgnore]
    public ICollection<BillDTO> Bills {get;set;}

}