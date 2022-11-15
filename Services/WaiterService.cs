using Microsoft.EntityFrameworkCore;
using ctds_webapi.Models;
using System.Globalization;

namespace ctds_webapi.Services;

public class WaiterService : IWaiterService
{
    OracleDBContext context;

    public WaiterService(OracleDBContext dbContext)
    {
        context = dbContext;
    }

    private void throwNotFoundException(Waiter waiter, Guid id)
    {
        if (waiter == null)
        {
            Exception ex = new Exception($"Waiter with id:{id} cannot be found");
            throw ex;
        }
    }

    public IEnumerable<Waiter> Get()
    {
        return context.Waiters;
    }

    public Waiter GetById(Guid id)
    {
        Waiter waiter = context.Waiters.Find(id);
        throwNotFoundException(waiter, id);

        return waiter;
    }

    public async Task Save(Waiter waiter)
    {
        waiter.Id = Guid.NewGuid();
        context.Waiters.Add(waiter);

        await context.SaveChangesAsync();
    }

    public async Task Update(Guid id, Waiter waiter)
    {
        var waiterToUpdate = context.Waiters.Find(id);
        throwNotFoundException(waiterToUpdate, id);

        waiterToUpdate.Name = waiter.Name;
        waiterToUpdate.LastName = waiter.LastName;
        waiterToUpdate.Age = waiter.Age;
        waiterToUpdate.Seniority = waiter.Seniority;

        await context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var waiterToDelete = context.Waiters.Find(id);
        throwNotFoundException(waiterToDelete, id);
        context.Waiters.Remove(waiterToDelete);

        await context.SaveChangesAsync();
    }

    public IEnumerable<TotalSellsByWaiter> TotalSells(DateTime startDate, DateTime endDate)
    {
        DateTimeFormatInfo dtfi = CultureInfo.CreateSpecificCulture("ja-JP").DateTimeFormat;
        var start_str = startDate.ToString("d", dtfi);
        var end_str = endDate.ToString("d", dtfi);

        Console.WriteLine(start_str);
        Console.WriteLine(end_str);


        // var TOTAL_SELLS_BY_WAITER = context.TotalSellsByWaiter.FromSqlInterpolated($"SELECT \"w\".\"Name\", \"w\".\"LastName\", sum(\"dBill\".\"Value\") AS WaiterSells FROM \"Waiter\" \"w\" INNER JOIN \"Bill\" \"b\" ON \"w\".\"Id\" = \"b\".\"WaiterId\" INNER JOIN \"Detail_Bill\" \"dBill\" ON \"b\".\"BillId\" = \"dBill\".\"BillId\" GROUP BY \"w\".\"Name\", \"w\".\"LastName\" ORDER BY sum(\"dBill\".\"Value\") DESC;");
        var TOTAL_SELLS_BY_WAITER = context.TotalSellsByWaiter.FromSqlInterpolated($"SELECT \"w\".\"Name\", \"w\".\"LastName\", \"b\".\"CreatedAt\", sum(\"dBill\".\"Value\") AS WaiterSells FROM \"Waiter\" \"w\" INNER JOIN \"Bill\" \"b\" ON \"w\".\"Id\" = \"b\".\"WaiterId\" INNER JOIN \"Detail_Bill\" \"dBill\" ON \"b\".\"BillId\" = \"dBill\".\"BillId\" GROUP BY \"w\".\"Name\", \"w\".\"LastName\", \"b\".\"CreatedAt\" HAVING (\"b\".\"CreatedAt\" >= TO_DATE({start_str}, 'YYYY/MM/DD') AND \"b\".\"CreatedAt\" <= TO_DATE({end_str},'YYYY/MM/DD')) ORDER BY sum(\"dBill\".\"Value\") DESC;");
        // var TOTAL_SELLS_BY_WAITER = context.TotalSellsByWaiter.FromSqlInterpolated($"SELECT \"w\".\"Name\", \"w\".\"LastName\", \"b\".\"CreatedAt\", sum(\"dBill\".\"Value\") AS WaiterSells FROM \"Waiter\" \"w\" INNER JOIN \"Bill\" \"b\" ON \"w\".\"Id\" = \"b\".\"WaiterId\" INNER JOIN \"Detail_Bill\" \"dBill\" ON \"b\".\"BillId\" = \"dBill\".\"BillId\" GROUP BY \"w\".\"Name\", \"w\".\"LastName\", \"b\".\"CreatedAt\" HAVING (\"b\".\"CreatedAt\" >= TO_DATE('2022/11/01', 'YYYY/MM/DD') AND \"b\".\"CreatedAt\" <= TO_DATE('2022/11/14','YYYY/MM/DD')) ORDER BY sum(\"dBill\".\"Value\") DESC;");
        // FIRST_ONE  // SELECT \"w\".\"Name\", \"w\".\"LastName\", sum(\"dBill\".\"Value\") AS WaiterSells FROM \"Waiter\" \"w\" INNER JOIN \"Bill\" \"b\" ON \"w\".\"Id\" = \"b\".\"WaiterId\" INNER JOIN \"Detail_Bill\" \"dBill\" ON \"b\".\"BillId\" = \"dBill\".\"BillId\" GROUP BY \"w\".\"Name\", \"w\".\"LastName\" HAVING (\"b\".\"CreatedAt\" >= {startDate} && \"b\".\"CreatedAt\" <= {endDate}) ORDER BY sum(\"dBill\".\"Value\") DESC;
        // LAST_ONE  // SELECT "w"."Name", "w"."LastName", "b"."CreatedAt", sum("dBill"."Value") AS WaiterSells FROM "Waiter" "w" INNER JOIN "Bill" "b" ON "w"."Id" = "b"."WaiterId" INNER JOIN "Detail_Bill" "dBill" ON "b"."BillId" = "dBill"."BillId" GROUP BY "w"."Name", "w"."LastName", "b"."CreatedAt" HAVING ("b"."CreatedAt" >= TO_DATE('2022/11/01', 'YYYY/MM/DD') AND "b"."CreatedAt" <= TO_DATE('2022/11/14','YYYY/MM/DD')) ORDER BY sum("dBill"."Value") DESC;
        return TOTAL_SELLS_BY_WAITER.ToList();
    }
}

public interface IWaiterService
{
    IEnumerable<Waiter> Get();
    Waiter GetById(Guid id);
    Task Save(Waiter waiter);
    Task Update(Guid id, Waiter waiter);
    Task Delete(Guid id);
    IEnumerable<TotalSellsByWaiter> TotalSells(DateTime startDate, DateTime endDate);

}