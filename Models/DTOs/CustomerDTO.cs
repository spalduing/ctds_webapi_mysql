using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ctds_webapi.Models;
public class CreateCustomerDTO
{
    [Required]
    public string Name {get;set;}
    [Required]
    public string LastName {get;set;}
    [Required]
    public string Address {get;set;}
    [Required]
    public long Cellphone {get;set;}
    public DateTime CreatedAt {get;set;}

}

public class UpdateCustomerDTO : CreateCustomerDTO
{

}

public class CustomerDTO : CreateCustomerDTO
{
    public Guid Id {get;set;}

    [JsonIgnore]
    public ICollection<BillDTO> Bills {get;set;}

}