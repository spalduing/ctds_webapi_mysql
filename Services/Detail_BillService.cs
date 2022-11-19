using Microsoft.EntityFrameworkCore;
using ctds_webapi.Models;

namespace ctds_webapi.Services;

public class Detail_BillService : IDetail_BillService
{
    OracleDBContext context;

    public Detail_BillService(OracleDBContext dbContext )
    {
        context = dbContext;
    }

    private void throwNotFoundException(Detail_Bill detailBill, Guid id)
    {
        if (detailBill == null)
        {
            Exception ex = new Exception($"Detail_Bill with id:{id} cannot be found");
            throw ex;
        }
    }

    public IEnumerable<Detail_Bill> Get()
    {
        return context.Detail_Bills;
    }

    public Detail_Bill GetById(Guid id)
    {
        // Detail_Bill detailBill = context.Detail_Bills.Find(id);
        Detail_Bill detailBill = context.Detail_Bills
                                .Include(dBill => dBill.Manager)
                                .Include(dBil => dBil.Bill).Where(dBill => dBill.DetailBilId.Equals(id))
                                .FirstOrDefault();

        throwNotFoundException(detailBill, id);

        return detailBill;
    }

    public async Task Save(Detail_Bill detailBill)
    {
        context.Detail_Bills.Add(detailBill);

        await context.SaveChangesAsync();
    }

    public async Task Update(Guid id, Detail_Bill detailBill)
    {
        var detailBillToUpdate = context.Detail_Bills.Find(id);
        throwNotFoundException(detailBillToUpdate, id);

        detailBillToUpdate.BillId = detailBill.BillId;
        detailBillToUpdate.ManagerId = detailBill.ManagerId;
        detailBillToUpdate.Dish = detailBill.Dish;
        detailBillToUpdate.Value = detailBill.Value;

        await context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var detailBillToDelete = context.Detail_Bills.Find(id);
        throwNotFoundException(detailBillToDelete, id);

        context.Detail_Bills.Remove(detailBillToDelete);

        await context.SaveChangesAsync();

    }
    // IMPORTANT_COMMENT: SERVICE WITH A TYPED NO-ASYNC APPROACH
    // AND USING res.Key.[PROP]
    public BestSellerProduct BestSellerProduct(DateTime startDate, DateTime endDate)
    {
        var bestSellerProductQuery = context.Detail_Bills
        .Where(res => res.CreatedAt >= startDate.Date && res.CreatedAt <= endDate.Date)
        .GroupBy(res => new { res.Dish, res.CreatedAt })
        .Select(res => new BestSellerProduct
        {
            Dish = res.Key.Dish,
            Amount = res.Select(x => x.Dish).Count(),
            CreatedAt = res.Key.CreatedAt,
            TotalBilled = res.Select(x => x.Value).Sum()
        })
        .OrderByDescending(x => x.TotalBilled)
        .ToList();

        var bestSellerProduct = bestSellerProductQuery.ElementAt(0);

        return bestSellerProduct;
    }
}

public interface IDetail_BillService
{
    IEnumerable<Detail_Bill> Get();
    Detail_Bill GetById(Guid id);
    Task Save(Detail_Bill detailBill);
    Task Update(Guid id, Detail_Bill detailBill);
    Task Delete(Guid id);
    BestSellerProduct BestSellerProduct(DateTime startDate, DateTime endDate);
}