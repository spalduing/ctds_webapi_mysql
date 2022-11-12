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
    private void throwNotFoundException(Bill bill, Guid id)
    {
        if (bill == null)
        {
            Exception ex = new Exception($"Bill with id:{id} cannot be found");
            throw ex;
        }
    }

    public IEnumerable<Bill> Get()
    {
        return context.Bills;
    }

    public Bill GetById(Guid id)
    {
        // Bill bill = context.Bills.Find(id);
        Bill bill = context.Bills
                    .Include(bill => bill.Customer)
                    .Include(bill => bill.Waiter)
                    .Include(bill => bill.Table)
                    .Include(bill => bill.Detail_Bills)
                    .Where(bill => bill.BillId.Equals(id)).FirstOrDefault();

        throwNotFoundException(bill, id);

        return bill;
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
        var billToUpdate = context.Bills.Find(id);
        throwNotFoundException(billToUpdate, id);

        billToUpdate.CreatedAt = bill.CreatedAt;
        billToUpdate.CustomerId = bill.CustomerId;
        billToUpdate.TableId = bill.TableId;
        billToUpdate.WaiterId = bill.WaiterId;

        await context.SaveChangesAsync();

    }

    public async Task Delete(Guid id)
    {
        var billToDelete = context.Bills.Find(id);
        throwNotFoundException(billToDelete, id);
        context.Bills.Remove(billToDelete);

        await context.SaveChangesAsync();
    }

}

public interface IBillService
{
    IEnumerable<Bill> Get();
    Bill GetById(Guid id);
    Task Save(Bill bill);
    Task Update(Guid id, Bill bill);
    Task Delete(Guid id);
    Task RegisterBill(Bill bill);
}