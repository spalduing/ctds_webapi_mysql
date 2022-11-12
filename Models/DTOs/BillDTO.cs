using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ctds_webapi.Models;
public class CreateBillDTO
{
    public Guid  CustomerId {get;set;}
    public Guid TableId {get;set;}
    public Guid WaiterId {get;set;}
    public DateTime CreatedAt {get;set;}
    public ICollection<CreateDetail_BillDTO> Detail_Bills {get;set;}
}

public class UpdateBillDTO : CreateBillDTO
{

}

public class BillDTO : CreateBillDTO
{
    public Guid BillId {get;set;}
    public CustomerDTO Customer {get;set;}

    public TableDTO Table {get;set;}

    public WaiterDTO Waiter {get;set;}

    [JsonIgnore]
    public ICollection<Detail_BillDTO> Detail_Bills {get;set;}

}