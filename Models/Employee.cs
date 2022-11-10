namespace ctds_webapi.Models;

public abstract class Employee : Person
{
    public int Age {get;set;}
    public Seniority Seniority {get;set;}
}

public enum Seniority
{
    Junior,
    Mid,
    Senior
}