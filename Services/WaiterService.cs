using Microsoft.EntityFrameworkCore;
using ctds_webapi.Models;
using System.Globalization;
using System.Collections;

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

    public async Task<IEnumerable> TotalSells(DateTime startDate, DateTime endDate)
    {
        var waiters = context.Waiters;
        var bills = context.Bills;
        var detailBills = context.Detail_Bills;

        var waitersConsumptionsQuery = await waiters
        .Join(bills, w => w.Id, b => b.WaiterId, (w, b) => new { w, b })
        .Join(detailBills, bdb => bdb.b.BillId, dB => dB.BillId, (bdb, dB) => new { bdb, dB })
        .Where(res => res.bdb.b.CreatedAt.Date >= startDate.Date && res.bdb.b.CreatedAt.Date <= endDate.Date)
        .GroupBy(res => new { res.bdb.w.Name, res.bdb.w.LastName, res.bdb.b.CreatedAt })
        .Select(res => new
        {
            res.Key.Name,
            res.Key.LastName,
            res.Key.CreatedAt,
            WaiterSells = res.Select(x => x.dB.Value).Sum()
        })
        .OrderByDescending(x => x.WaiterSells)
        .ToListAsync();

        return waitersConsumptionsQuery;
    }
}

public interface IWaiterService
{
    IEnumerable<Waiter> Get();
    Waiter GetById(Guid id);
    Task Save(Waiter waiter);
    Task Update(Guid id, Waiter waiter);
    Task Delete(Guid id);
    Task<IEnumerable> TotalSells(DateTime startDate, DateTime endDate);

}