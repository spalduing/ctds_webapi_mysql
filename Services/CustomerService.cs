using Microsoft.EntityFrameworkCore;
using ctds_webapi.Models;
using System.Globalization;

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

    public IEnumerable<ConsumptionsByValue> CustomersConsumptions(double givenValue, DateTime startDate, DateTime endDate)
    {
        DateTimeFormatInfo dtfi = CultureInfo.CreateSpecificCulture("ja-JP").DateTimeFormat;
        var start_str = startDate.ToString("d", dtfi);
        var end_str = endDate.ToString("d", dtfi);

        var CUSTOMERS_CONSUMPTION_BY_VALUE = context.CustomerConsumptionsByValue.FromSqlInterpolated($"SELECT \"c\".\"Name\", \"c\".\"LastName\", sum(\"dBill\".\"Value\") AS CustomerConsumption FROM \"Customer\" \"c\" INNER JOIN \"Bill\" \"b\" ON \"c\".\"Id\" = \"b\".\"CustomerId\" INNER JOIN \"Detail_Bill\" \"dBill\" ON \"b\".\"BillId\" = \"dBill\".\"BillId\" GROUP BY \"c\".\"Id\", \"c\".\"Name\", \"c\".\"LastName\" HAVING sum(\"dBill\".\"Value\") >= {givenValue} ORDER BY sum(\"dBill\".\"Value\") DESC;");
        // var CUSTOMERS_CONSUMPTION_BY_VALUE = context.CustomerConsumptionsByValue.FromSqlInterpolated($"SELECT \"c\".\"Name\", \"c\".\"LastName\", \"b\".\"CreatedAt\", sum(\"dBill\".\"Value\") AS CustomerConsumption FROM \"Customer\" \"c\" INNER JOIN \"Bill\" \"b\" ON \"c\".\"Id\" = \"b\".\"CustomerId\" INNER JOIN \"Detail_Bill\" \"dBill\" ON \"b\".\"BillId\" = \"dBill\".\"BillId\" GROUP BY \"c\".\"Id\", \"c\".\"Name\", \"c\".\"LastName\", \"b\".\"CreatedAt\" HAVING (sum(\"dBill\".\"Value\") >= {givenValue} AND (\"b\".\"CreatedAt\" >= TO_DATE({start_str}, 'YYYY/MM/DD') AND \"b\".\"CreatedAt\" <= TO_DATE({end_str},'YYYY/MM/DD'))) ORDER BY sum(\"dBill\".\"Value\") DESC;");

        // SELECT "c"."Name", "c"."LastName", "b"."CreatedAt", sum("dBill"."Value") AS CustomerConsumption FROM "Customer" "c" INNER JOIN "Bill" "b" ON "c"."Id" = "b"."CustomerId" INNER JOIN "Detail_Bill" "dBill" ON "b"."BillId" = "dBill"."BillId" GROUP BY "c"."Id", "c"."Name", "c"."LastName", "b"."CreatedAt" HAVING sum("dBill"."Value") >= 4.4 ORDER BY sum("dBill"."Value") DESC;
        // SELECT "c"."Name", "c"."LastName", "b"."CreatedAt", sum("dBill"."Value") AS CustomerConsumption FROM "Customer" "c" INNER JOIN "Bill" "b" ON "c"."Id" = "b"."CustomerId" INNER JOIN "Detail_Bill" "dBill" ON "b"."BillId" = "dBill"."BillId" GROUP BY "c"."Id", "c"."Name", "c"."LastName", "b"."CreatedAt" HAVING (sum("dBill"."Value") >= 4.4 AND ("b"."CreatedAt" >= TO_DATE('2022/11/01', 'YYYY/MM/DD') AND "b"."CreatedAt" <= TO_DATE('2022/11/14','YYYY/MM/DD'))) ORDER BY sum("dBill"."Value") DESC;
        return CUSTOMERS_CONSUMPTION_BY_VALUE.ToList();
    }

}

public interface ICustomerService
{
    IEnumerable<Customer> Get();
    Customer GetById(Guid id);
    Task Save(Customer customer);
    Task Update(Guid id, Customer customer);
    Task Delete(Guid id);
    IEnumerable<ConsumptionsByValue> CustomersConsumptions(double givenValue, DateTime startDate, DateTime endDate);
}


