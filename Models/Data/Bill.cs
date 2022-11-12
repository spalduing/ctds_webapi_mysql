using System.Text.Json.Serialization;

namespace ctds_webapi.Models;

public class Bill
{
    public Guid BillId {get;set;}
    public Guid  CustomerId {get;set;}
    public Guid TableId {get;set;}
    public Guid WaiterId {get;set;}
    public DateTime CreatedAt {get;set;}

    public virtual Customer Customer {get;set;}

    public virtual Table Table {get;set;}

    public virtual Waiter Waiter {get;set;}

    [JsonIgnore]
    public virtual ICollection<Detail_Bill> Detail_Bills {get;set;}

    [JsonIgnore]
    public double TotalValue {get;set;}
    // public double TotalValue {
    //     get{
    //         double totalVal = 0.0;
    //         if(Detail_Bills.Any() != false)
    //         {
    //             foreach (var detailBill in Detail_Bills)
    //             {
    //                 totalVal += detailBill.Value;
    //             }
    //         }
    //         return totalVal;
    //     }

    //     set{}
    // }
}