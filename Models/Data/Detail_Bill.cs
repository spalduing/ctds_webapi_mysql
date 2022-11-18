using System.Text.Json.Serialization;

namespace ctds_webapi.Models;

public class Detail_Bill : Auditable
{
    public Guid DetailBilId {get;set;}
    public Guid BillId {get;set;}
    public Guid ManagerId {get;set;}
    public Dish Dish {get;set;}
    public double Value {get;set;}


    public virtual Bill Bill {get;set;}
    public virtual Manager Manager {get;set;}

}

public enum Dish
{
    MILK,
    MEAT,
    ICE_CREAM,
    FRIED_CHICKEN,
    CHICHARRON
}