namespace ctds_webapi.Models;
public abstract class  Person : Auditable
{
    public Guid Id {get;set;}
    public string Name {get;set;}
    public string LastName {get;set;}
}