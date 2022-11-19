using Microsoft.EntityFrameworkCore;
using ctds_webapi.Models;
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

    // IMPORTANT_COMMENT: QUERY WITH An ASYNC TASK APPROACH
    // AND USING re.Select(x=>x.[GROUP].[GROUP].[PROP]).FirstOrDefault()
    public async Task<IEnumerable> TotalSells(DateTime startDate, DateTime endDate)
    {
        var waiters = context.Waiters;
        var bills = context.Bills;
        var detailBills = context.Detail_Bills;

        var waitersConsumptionsQuery = await waiters
        .Join(bills, w => w.Id, b => b.WaiterId, (w, b) => new { w, b })
        .Where(res => res.b.CreatedAt.Date >= startDate.Date && res.b.CreatedAt.Date <= endDate.Date)
        .GroupJoin(detailBills, bdb => bdb.b.BillId, dB => dB.BillId, (bdb, dB) => new { bdb, dB })
        .SelectMany( x => x.dB.DefaultIfEmpty(),
        (res,dB ) => new
        {
            Name = res.bdb.w.Name,
            LastName = res.bdb.w.LastName,
            CreatedAt = res.bdb.w.CreatedAt,
            Value = dB == null ? 0.0 : dB.Value,
        })
        .GroupBy(x => new { x.Name, x.LastName, x.CreatedAt})
        .Select(gres => new
        TotalSellsByWaiter
        {
            Name= gres.Key.Name,
            LastName= gres.Key.LastName,
            CreatedAt= gres.Key.CreatedAt,
            WaiterSells = gres.Select(x => x.Value).Sum()
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