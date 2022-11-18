using Microsoft.EntityFrameworkCore;
using ctds_webapi.Models;
using AutoMapper;

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

    // public async Task<IEnumerable> CustomersConsumptions(double givenValue, DateTime startDate, DateTime endDate)

    public BestSellerProduct BestSellerProduct(DateTime startDate, DateTime endDate)
    {
        var bestSellerProductQuery = context.Detail_Bills
        .Where(res => res.CreatedAt >= startDate.Date && res.CreatedAt <= endDate.Date)
        .GroupBy(res => new { res.Dish, res.CreatedAt })
        .Select(res => new
        {
            res.Key.Dish,
            res.Key.CreatedAt,
            Amount = res.Select(x => x.Dish).Count(),
            TotalBilled = res.Select(x => x.Value).Sum()
        })
        .OrderByDescending(x => x.TotalBilled)
        .ToList();

        var BEST_SELLER_PRODUCT = new BestSellerProduct();
        BEST_SELLER_PRODUCT.Amount = bestSellerProductQuery.ElementAt(0).Amount;
        BEST_SELLER_PRODUCT.CreatedAt = bestSellerProductQuery.ElementAt(0).CreatedAt;
        BEST_SELLER_PRODUCT.Dish = bestSellerProductQuery.ElementAt(0).Dish;
        BEST_SELLER_PRODUCT.TotalBilled = bestSellerProductQuery.ElementAt(0).TotalBilled;

        return BEST_SELLER_PRODUCT;

        // DateTimeFormatInfo dtfi = CultureInfo.CreateSpecificCulture("ja-JP").DateTimeFormat;
        // var start_str = startDate.ToString("d", dtfi);
        // var end_str = endDate.ToString("d", dtfi);

        // var BEST_SELLER_PRODUCT = context.BestSellerProducts.FromSqlInterpolated($"SELECT \"d\".\"Dish\", \"d\".\"CreatedAt\", count(\"d\".\"Dish\") AS Amount, sum(\"d\".\"Value\") AS TotalBilled FROM \"Detail_Bill\" \"d\" GROUP BY \"d\".\"Dish\" ORDER BY count(\"d\".\"Dish\") DESC;");
        // var BEST_SELLER_PRODUCT = context.BestSellerProducts.FromSqlInterpolated($"SELECT \"d\".\"Dish\", \"b\".\"CreatedAt\", count(\"d\".\"Dish\") AS Amount, sum(\"d\".\"Value\") AS TotalBilled FROM \"Detail_Bill\" \"d\" INNER JOIN \"Bill\" \"b\" ON \"d\".\"BillId\" = \"b\".\"BillId\" GROUP BY \"d\".\"Dish\", \"b\".\"CreatedAt\" HAVING (\"b\".\"CreatedAt\" >= TO_DATE({start_str}, 'YYYY/MM/DD') AND \"b\".\"CreatedAt\" <= TO_DATE({end_str},'YYYY/MM/DD')) ORDER BY count(\"d\".\"Dish\") DESC;");

        // var BEST_SELLER_PRODUCT = context.BestSellerProducts.FromSqlInterpolated($"SELECT \"d\".\"Dish\", count(\"d\".\"Dish\") AS Amount, sum(\"d\".\"Value\") AS TotalBilled, \"b\".\"CreatedAt\" FROM \"Detail_Bill\" \"d\" INNER JOIN \"Bill\" \"b\" ON \"d\".\"BillId\" = \"b\".\"BillId\" GROUP BY \"d\".\"Dish\", \"b\".\"CreatedAt\" HAVING (\"b\".\"CreatedAt\" >= TO_DATE({start_str}, 'YYYY/MM/DD') AND \"b\".\"CreatedAt\" <= TO_DATE({end_str},'YYYY/MM/DD')) ORDER BY count(\"d\".\"Dish\") DESC;");
        // SELECT "d"."Dish", "b"."CreatedAt", count("d"."Dish") AS Amount, sum("d"."Value") AS TotalBilled FROM "Detail_Bill" "d" INNER JOIN "Bill" "b" ON "d"."BillId" = "b"."BillId" GROUP BY "d"."Dish", "b"."CreatedAt" HAVING ("b"."CreatedAt" >= TO_DATE('2022/11/01', 'YYYY/MM/DD') AND "b"."CreatedAt" <= TO_DATE('2022/11/14','YYYY/MM/DD')) ORDER BY count("d"."Dish") DESC;
        // return BEST_SELLER_PRODUCT.ToList().ElementAt(0);
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