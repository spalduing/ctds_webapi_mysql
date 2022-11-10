using Microsoft.EntityFrameworkCore;
using ctds_webapi.Models;

namespace ctds_webapi.Services;

public class Detail_BillService : IDetail_BillService
{
    OracleDBContext context;

    public Detail_BillService(OracleDBContext dbContext)
    {
        context = dbContext;
    }

    public IEnumerable<Detail_Bill> Get()
    {
        return context.Detail_Bills;
    }

    public async Task Save(Detail_Bill detail_bill)
    {
        context.Detail_Bills.Add(detail_bill);

        await context.SaveChangesAsync();
    }

    public async Task Update(Guid id, Detail_Bill detail_bill)
    {
        var detail_billToUpdate = context.Detail_Bills.Find(id);

        if(detail_billToUpdate != null)
        {
            await context.SaveChangesAsync();
        }
    }

    public async Task Delete(Guid id)
    {
        var detail_billToDelete = context.Detail_Bills.Find(id);

        if(detail_billToDelete !=null){
            context.Detail_Bills.Remove(detail_billToDelete);

            await context.SaveChangesAsync();
        }
    }

    public BestSellerProduct BestSellerProduct()
    {
        var BEST_SELLER_PRODUCT = context.BestSellerProducts.FromSqlInterpolated($"SELECT \"d\".\"Dish\", count(\"d\".\"Dish\") AS Amount, sum(\"d\".\"Value\") AS TotalBilled FROM \"Detail_Bill\" \"d\" GROUP BY \"d\".\"Dish\" ORDER BY count(\"d\".\"Dish\") DESC;");
        // Console.WriteLine($"###BEST_SELLER_PRODUCT => {BEST_SELLER_PRODUCT.Dish} | {BEST_SELLER_PRODUCT.Amount} | {BEST_SELLER_PRODUCT.TotalBilled}");

        return BEST_SELLER_PRODUCT.ToList().ElementAt(0);
    }
}

public interface IDetail_BillService
{
    IEnumerable<Detail_Bill> Get();
    Task Save(Detail_Bill detail_bill);
    Task Update(Guid id, Detail_Bill detail_bill);
    Task Delete(Guid id);
    BestSellerProduct BestSellerProduct();
}