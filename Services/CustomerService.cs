using Microsoft.EntityFrameworkCore;
using ctds_webapi.Models;
using System.Data;
using System.Collections;

namespace ctds_webapi.Services;

public class CustomerService : ICustomerService
{
    OracleDBContext context;

    public CustomerService(OracleDBContext dbContext)
    {
        context = dbContext;
    }

    private void throwNotFoundException(Customer customer, Guid id)
    {
        if (customer == null)
        {
            Exception ex = new Exception($"Customer with id:{id} cannot be found");
            throw ex;
        }
    }

    public IEnumerable<Customer> Get()
    {
        return context.Customers;
    }

    public Customer GetById(Guid id)
    {
        Customer customer = context.Customers.Find(id);
        throwNotFoundException(customer, id);

        return customer;
    }
    public async Task Save(Customer customer)
    {
        context.Customers.Add(customer);
        await context.SaveChangesAsync();
    }

    public async Task Update(Guid id, Customer customer)
    {
        var customerToUpdate = context.Customers.Find(id);
        throwNotFoundException(customerToUpdate, id);

        customerToUpdate.Name = customer.Name;
        customerToUpdate.LastName = customer.LastName;
        customerToUpdate.Address = customer.Address;
        customerToUpdate.Cellphone = customer.Cellphone;

        await context.SaveChangesAsync();

    }

    public async Task Delete(Guid id)
    {
        var customerToDelete = context.Customers.Find(id);
        throwNotFoundException(customerToDelete, id);

        context.Customers.Remove(customerToDelete);

        await context.SaveChangesAsync();
    }

    // IMPORTANT_COMMENT: QUERY WITH An ASYNC TASK APPROACH
    // AND USING res.Key.[PROP]
    public async Task<IEnumerable> CustomersConsumptions(double givenValue, DateTime startDate, DateTime endDate)
    {
        var customers = context.Customers;
        var bills = context.Bills;
        var detailBills = context.Detail_Bills;

        var customersConsumptionsQuery = await customers
        .Join(bills, c => c.Id, b => b.CustomerId, (c, b) => new { c, b })
        .Join(detailBills, bdb => bdb.b.BillId, dB => dB.BillId, (bdb, dB) => new { bdb, dB })
        .Where(res => res.bdb.b.CreatedAt.Date >= startDate.Date && res.bdb.b.CreatedAt.Date <= endDate.Date)
        .GroupBy(res => new { res.bdb.c.Name, res.bdb.c.LastName, res.bdb.b.CreatedAt })
        .Select(res => new ConsumptionsByValue
        {
            Name = res.Key.Name,
            LastName = res.Key.LastName,
            CreatedAt= res.Key.CreatedAt,
            CustomerConsumption = res.Select(x => x.dB.Value).Sum()
        })
        .Where(res => res.CustomerConsumption >= givenValue )
        .OrderByDescending(x => x.CustomerConsumption)
        .ToListAsync();

        return customersConsumptionsQuery;
    }


}


public interface ICustomerService
{
    IEnumerable<Customer> Get();
    Customer GetById(Guid id);
    Task Save(Customer customer);
    Task Update(Guid id, Customer customer);
    Task Delete(Guid id);
    Task<IEnumerable> CustomersConsumptions(double givenValue, DateTime startDate, DateTime endDate);
}


