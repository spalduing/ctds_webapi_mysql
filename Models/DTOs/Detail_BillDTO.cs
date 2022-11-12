using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ctds_webapi.Models;
public class CreateDetail_BillDTO
{
    [Required]
    public Guid BillId {get;set;}

    [Required]
    public Guid ManagerId {get;set;}

    [Required]
    public Dish Dish {get;set;}

    [Required]
    public double Value {get;set;}
}

public class UpdateDetail_BillDTO : CreateDetail_BillDTO
{

}

public class Detail_BillDTO : CreateDetail_BillDTO
{
    public Guid DetailBilId {get;set;}
    public BillDTO Bill {get;set;}
    public ManagerDTO Manager {get;set;}

}