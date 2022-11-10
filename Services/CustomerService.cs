using Microsoft.EntityFrameworkCore;
using ctds_webapi.Models;

namespace ctds_webapi.Services;

public class CustomerService : ICustomerService
{
    OracleDBContext context;

    public CustomerService(OracleDBContext dbContext)
    {
        context = dbContext;
    }

    public IEnumerable<Customer> Get()
    {
        return context.Customers;
    }

    public async Task Save(Customer customer)
    {
        context.Customers.Add(customer);
        await context.SaveChangesAsync();
    }

    public async Task Update(Guid id, Customer customer)
    {
        var customerToUpdate = context.Customers.Find(id);

        if (customerToUpdate != null)
        {
            customerToUpdate.Name = customer.Name;
            customerToUpdate.LastName = customer.LastName;
            customerToUpdate.Address = customer.Address;
            customerToUpdate.Cellphone = customer.Cellphone;

            await context.SaveChangesAsync();
        }
    }

    public async Task Delete(Guid id)
    {
        var customerToDelete = context.Customers.Find(id);

        if (customerToDelete != null)
        {
            context.Customers.Remove(customerToDelete);

            await context.SaveChangesAsync();
        }
    }

    public IEnumerable<Waiter> TotalSellsByWaiter()
    {
        var waiters = context.Waiters.Where(p => p.Seniority == Seniority.Junior).ToList<Waiter>();
        return waiters;

    }

    public IEnumerable<ConsumptionsByValue> CustomersConsumptions(double givenValue)
    {
        var CUSTOMERS_CONSUMPTION_BY_VALUE = context.CustomerConsumptionsByValue.FromSqlInterpolated($"SELECT \"c\".\"Name\", \"c\".\"LastName\", sum(\"dBill\".\"Value\") AS CustomerConsumption FROM \"Customer\" \"c\" INNER JOIN \"Bill\" \"b\" ON \"c\".\"Id\" = \"b\".\"CustomerId\" INNER JOIN \"Detail_Bill\" \"dBill\" ON \"b\".\"BillId\" = \"dBill\".\"BillId\" GROUP BY \"c\".\"Id\", \"c\".\"Name\", \"c\".\"LastName\" HAVING sum(\"dBill\".\"Value\") >= {givenValue} ORDER BY sum(\"dBill\".\"Value\") DESC;");

        return CUSTOMERS_CONSUMPTION_BY_VALUE.ToList();
    }

}

public interface ICustomerService
{
    IEnumerable<Customer> Get();
    Task Save(Customer customer);
    Task Update(Guid id, Customer customer);
    Task Delete(Guid id);
    IEnumerable<ConsumptionsByValue> CustomersConsumptions(double givenValue);
}


