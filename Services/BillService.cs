using Microsoft.EntityFrameworkCore;
using ctds_webapi.Models;

namespace ctds_webapi.Services;

public class BillService : IBillService
{
    OracleDBContext context;

    public BillService(OracleDBContext dbContext)
    {
        context = dbContext;
    }

    public IEnumerable<Bill> Get()
    {
        return context.Bills;
        // return context.Bills.Include(bill => bill.Customer).Include(bill => bill.Waiter).Include(bill => bill.Table);
    }

    public async Task Save(Bill bill)
    {
        context.Bills.Add(bill);
        await context.SaveChangesAsync();
    }

    public async Task RegisterBill(Bill bill)
    {

        using (var transaction = context.Database.BeginTransaction())
        {
            // bill.BillId = Guid.NewGuid();
            bill.CreatedAt = DateTime.Now;
            context.Bills.Add(bill);
            foreach (var detailBill in bill.Detail_Bills.ToList())
            {
                // detailBill.DetailBilId = Guid.NewGuid();
                detailBill.BillId = bill.BillId;
            }
            // context.BulkInsert(bill.Detail_Bills.ToList());
            context.AddRange(bill.Detail_Bills.ToList());
            await context.SaveChangesAsync();
            transaction.Commit();
        }
    }

    public async Task Update(Guid id, Bill bill)
    {
        var managerToUpdate = context.Bills.Find(id);

        if (managerToUpdate != null)
        {
            // NYI //
            await context.SaveChangesAsync();
        }
    }

    public async Task Delete(Guid id)
    {
        var billToDelete = context.Bills.Find(id);

        if (billToDelete != null)
        {
            context.Bills.Remove(billToDelete);

            await context.SaveChangesAsync();
        }
    }

}

public interface IBillService
{
    IEnumerable<Bill> Get();
    Task Save(Bill bill);
    Task Update(Guid id, Bill bill);
    Task Delete(Guid id);
    Task RegisterBill(Bill bill);
}