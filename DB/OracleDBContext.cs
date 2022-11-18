using Microsoft.EntityFrameworkCore;
using ctds_webapi.Models;

namespace ctds_webapi;
public class OracleDBContext : DbContext
{
    public DbSet<Customer> Customers {get;set;}
    public DbSet<Waiter> Waiters {get;set;}
    public DbSet<Manager> Managers {get;set;}
    public DbSet<Table> Tables {get;set;}
    public DbSet<Bill> Bills {get;set;}
    public DbSet<Detail_Bill> Detail_Bills {get;set;}
    public DbSet<ConsumptionsByValue> CustomerConsumptionsByValue {get;set;}
    public DbSet<TotalSellsByWaiter> TotalSellsByWaiter {get;set;}
    public DbSet<BestSellerProduct> BestSellerProducts {get;set;}

    public OracleDBContext(DbContextOptions<OracleDBContext> options) : base(options) {}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<TotalSellsByWaiter>(waitersTotalSells => {
            waitersTotalSells.HasNoKey();
            waitersTotalSells.ToView("TotalSellsByWaiter");
        });
        modelBuilder.Entity<ConsumptionsByValue>(customerConsumptions => {
            customerConsumptions.HasNoKey();
            customerConsumptions.ToView("CustomerConsumptionsByValue");
        });
        modelBuilder.Entity<BestSellerProduct>(bestSellerProduct => {
            bestSellerProduct.HasNoKey();
            bestSellerProduct.ToView("BestSellerProduct");
        });


        List<Customer> customerList = new List<Customer>();

        customerList.Add(new Customer() { Id=Guid.Parse("80bb2ae8-d779-4247-a8d3-2270b4ec68d1"),
                                       Name="Ron",
                                       LastName="Danger",
                                       Address="8th street",
                                       Cellphone=3224657628,
                                       CreatedAt = new DateTime(2022,10,28)  });
        customerList.Add(new Customer() { Id=Guid.Parse("80bb2ae8-d779-4247-a8d3-2270b4ec68d2"),
                                       Name="John",
                                       LastName="Doe",
                                       Address="7th street",
                                       Cellphone=3224657628,
                                       CreatedAt = new DateTime(2022,10,28)  });
        customerList.Add(new Customer() { Id=Guid.Parse("80bb2ae8-d779-4247-a8d3-2270b4ec68d3"),
                                       Name="Foo",
                                       LastName="bar",
                                       Address="6th street",
                                       Cellphone=3224657628,
                                       CreatedAt = new DateTime(2022,10,28)  });


        modelBuilder.Entity<Customer>( customer =>
        {
            customer.ToTable("Customer");
            customer.HasKey(p => p.Id);
            customer.Property(p => p.Name);
            customer.Property(p => p.LastName);
            customer.Property(p => p.Address).IsRequired(false);
            customer.Property(p => p.Cellphone);
            customer.Property(p => p.CreatedAt);

            customer.HasData(customerList);
        });

        List<Waiter> waiterList = new List<Waiter>();

        waiterList.Add(new Waiter() { Id=Guid.Parse("133e9d8d-7bbc-4e23-93c4-cb8de085919f"),
                                    Name="Michael",
                                    LastName="Mikella",
                                    Age=28,
                                    Seniority=Seniority.Mid,
                                    CreatedAt = new DateTime(2022,10,28) });
        waiterList.Add(new Waiter() { Id=Guid.Parse("233e9d8d-7bbc-4e23-93c4-cb8de085919f"),
                                    Name="Hoarahlux",
                                    LastName="Warrior",
                                    Age=28,
                                    Seniority=Seniority.Junior,
                                    CreatedAt = new DateTime(2022,10,28) });
        waiterList.Add(new Waiter() { Id=Guid.Parse("333e9d8d-7bbc-4e23-93c4-cb8de085919f"),
                                    Name="Rex",
                                    LastName="Guerrero",
                                    Age=28,
                                    Seniority=Seniority.Senior,
                                    CreatedAt = new DateTime(2022,10,28) });

        modelBuilder.Entity<Waiter>( waiter =>
        {
            waiter.ToTable("Waiter");
            waiter.HasKey(p => p.Id);
            waiter.Property(p => p.Name);
            waiter.Property(p => p.LastName);
            waiter.Property(p => p.Age);
            waiter.Property(p => p.Seniority);
            waiter.Property(p => p.CreatedAt);


            waiter.HasData(waiterList);
        });

        List<Table> tableList = new List<Table>();

        tableList.Add(new Table(){ TableId=Guid.Parse("b80c2655-5a22-4ba2-94a1-688a85d6d91b"),
                                 Name="1",
                                 Reserved=true,
                                 Stalls=4,
                                 CreatedAt = new DateTime(2022,10,28) });
        tableList.Add(new Table(){ TableId=Guid.Parse("b80c2655-5a22-4ba2-94a1-688a85d6d92b"),
                                 Name="2",
                                 Reserved=true,
                                 Stalls=6,
                                 CreatedAt = new DateTime(2022,1,28) });
        tableList.Add(new Table(){ TableId=Guid.Parse("b80c2655-5a22-4ba2-94a1-688a85d6d93b"),
                                 Name="3",
                                 Reserved=true,
                                 Stalls=4,
                                 CreatedAt = new DateTime(2022,7,28) });

        modelBuilder.Entity<Table>( table =>
        {
            table.ToTable("Table");
            table.Property(p => p.TableId);
            table.Property(p => p.Name);
            table.Property(p => p.Reserved);
            table.Property(p => p.Stalls);
            table.Property(p => p.CreatedAt);


            table.HasData(tableList);
        });

        List<Manager> managerList = new List<Manager>();

        managerList.Add(new Manager(){ Id=Guid.Parse("1cfe8617-8d05-46bd-8ad5-974488f1fe3c"),
                                     Name="Juan",
                                     LastName="Quiñones",
                                     Age=20,
                                     Seniority=Seniority.Junior,
                                     CreatedAt = new DateTime(2022,4,28) });
        managerList.Add(new Manager(){ Id=Guid.Parse("2cfe8617-8d05-46bd-8ad5-974488f1fe3c"),
                                     Name="Mike",
                                     LastName="Brando",
                                     Age=20,
                                     Seniority=Seniority.Junior,
                                     CreatedAt = new DateTime(2022,5,28) });
        managerList.Add(new Manager(){ Id=Guid.Parse("3cfe8617-8d05-46bd-8ad5-974488f1fe3c"),
                                     Name="Vito",
                                     LastName="Corleone",
                                     Age=20,
                                     Seniority=Seniority.Junior,
                                     CreatedAt = new DateTime(2022,5,28) });

        modelBuilder.Entity<Manager>( manager =>
        {
            manager.ToTable("Manager");
            manager.HasKey(p => p.Id);
            manager.Property(p => p.Name);
            manager.Property(p => p.LastName);
            manager.Property(p => p.Age);
            manager.Property(p => p.Seniority);
            manager.Property(p => p.CreatedAt);


            manager.HasData(managerList);
        });

        List<Bill> billList = new List<Bill>();

        billList.Add(new Bill(){ BillId=Guid.Parse("ef91c997-d758-44c6-8b9f-a66d7027e21c"),
                                CustomerId=Guid.Parse("80bb2ae8-d779-4247-a8d3-2270b4ec68d1"),
                                WaiterId=Guid.Parse("133e9d8d-7bbc-4e23-93c4-cb8de085919f"),
                                TableId=Guid.Parse("b80c2655-5a22-4ba2-94a1-688a85d6d91b"),
                                CreatedAt=new DateTime(2022,10,28)});
        billList.Add(new Bill(){ BillId=Guid.Parse("ef91c997-d758-44c6-8b9f-a66d7027e22c"),
                                CustomerId=Guid.Parse("80bb2ae8-d779-4247-a8d3-2270b4ec68d1"),
                                WaiterId=Guid.Parse("233e9d8d-7bbc-4e23-93c4-cb8de085919f"),
                                TableId=Guid.Parse("b80c2655-5a22-4ba2-94a1-688a85d6d92b"),
                                CreatedAt=new DateTime(2022,10,28)});
        billList.Add(new Bill(){ BillId=Guid.Parse("ef91c997-d758-44c6-8b9f-a66d7027e23c"),
                                CustomerId=Guid.Parse("80bb2ae8-d779-4247-a8d3-2270b4ec68d1"),
                                WaiterId=Guid.Parse("333e9d8d-7bbc-4e23-93c4-cb8de085919f"),
                                TableId=Guid.Parse("b80c2655-5a22-4ba2-94a1-688a85d6d93b"),
                                CreatedAt=new DateTime(2022,10,28)});
        billList.Add(new Bill(){ BillId=Guid.Parse("ef91c997-d758-44c6-8b9f-a66d7027e24c"),
                                CustomerId=Guid.Parse("80bb2ae8-d779-4247-a8d3-2270b4ec68d2"),
                                WaiterId=Guid.Parse("133e9d8d-7bbc-4e23-93c4-cb8de085919f"),
                                TableId=Guid.Parse("b80c2655-5a22-4ba2-94a1-688a85d6d91b"),
                                CreatedAt=new DateTime(2022,10,28)});
        billList.Add(new Bill(){ BillId=Guid.Parse("ef91c997-d758-44c6-8b9f-a66d7027e25c"),
                                CustomerId=Guid.Parse("80bb2ae8-d779-4247-a8d3-2270b4ec68d3"),
                                WaiterId=Guid.Parse("133e9d8d-7bbc-4e23-93c4-cb8de085919f"),
                                TableId=Guid.Parse("b80c2655-5a22-4ba2-94a1-688a85d6d93b"),
                                CreatedAt=new DateTime(2022,10,28)});

        modelBuilder.Entity<Bill>( bill =>
        {
            bill.ToTable("Bill");

            bill.HasKey(p => p.BillId);
            bill.Property(p => p.CustomerId);
            bill.Property(p => p.WaiterId);
            bill.Property(p => p.TableId);
            bill.Property(p => p.CreatedAt);
            bill.Ignore(p => p.TotalValue);

            bill.HasOne(p => p.Customer).WithMany(p => p.Bills).HasForeignKey(p => p.CustomerId);
            bill.HasOne(p => p.Waiter).WithMany(p => p.Bills).HasForeignKey(p => p.WaiterId);
            bill.HasOne(p => p.Table).WithMany(p => p.Bills).HasForeignKey(p => p.TableId);

            bill.HasData(billList);
        });

        List<Detail_Bill> detailBillList = new List<Detail_Bill>();

        detailBillList.Add(new Detail_Bill() {DetailBilId=Guid.Parse("d1d42909-a120-44b8-9fe0-ecc99d643161"),
                                            ManagerId=Guid.Parse("1cfe8617-8d05-46bd-8ad5-974488f1fe3c"),
                                            BillId=Guid.Parse("ef91c997-d758-44c6-8b9f-a66d7027e21c"),
                                            Dish=Dish.MEAT,
                                            Value=4.4, CreatedAt=new DateTime(2022,10,28) });
        detailBillList.Add(new Detail_Bill() {DetailBilId=Guid.Parse("d1d42909-a120-44b8-9fe0-ecc99d643162"),
                                            ManagerId=Guid.Parse("1cfe8617-8d05-46bd-8ad5-974488f1fe3c"),
                                            BillId=Guid.Parse("ef91c997-d758-44c6-8b9f-a66d7027e22c"),
                                            Dish=Dish.CHICHARRON,
                                            Value=4.4, CreatedAt=new DateTime(2022,10,28) });
        detailBillList.Add(new Detail_Bill() {DetailBilId=Guid.Parse("d1d42909-a120-44b8-9fe0-ecc99d643164"),
                                            ManagerId=Guid.Parse("3cfe8617-8d05-46bd-8ad5-974488f1fe3c"),
                                            BillId=Guid.Parse("ef91c997-d758-44c6-8b9f-a66d7027e24c"),
                                            Dish=Dish.CHICHARRON,
                                            Value=4.4, CreatedAt=new DateTime(2022,10,28) });
        detailBillList.Add(new Detail_Bill() {DetailBilId=Guid.Parse("d1d42909-a120-44b8-9fe0-ecc99d643165"),
                                            ManagerId=Guid.Parse("1cfe8617-8d05-46bd-8ad5-974488f1fe3c"),
                                            BillId=Guid.Parse("ef91c997-d758-44c6-8b9f-a66d7027e24c"),
                                            Dish=Dish.CHICHARRON,
                                            Value=4.4, CreatedAt=new DateTime(2022,10,28) });
        detailBillList.Add(new Detail_Bill() {DetailBilId=Guid.Parse("d1d42909-a120-44b8-9fe0-ecc99d643166"),
                                            ManagerId=Guid.Parse("2cfe8617-8d05-46bd-8ad5-974488f1fe3c"),
                                            BillId=Guid.Parse("ef91c997-d758-44c6-8b9f-a66d7027e22c"),
                                            Dish=Dish.MILK,
                                            Value=4.4, CreatedAt=new DateTime(2022,10,28) });
        detailBillList.Add(new Detail_Bill() {DetailBilId=Guid.Parse("d1d42909-a120-44b8-9fe0-ecc99d643167"),
                                            ManagerId=Guid.Parse("3cfe8617-8d05-46bd-8ad5-974488f1fe3c"),
                                            BillId=Guid.Parse("ef91c997-d758-44c6-8b9f-a66d7027e21c"),
                                            Dish=Dish.ICE_CREAM,
                                            Value=4.4, CreatedAt=new DateTime(2022,10,28) });
        detailBillList.Add(new Detail_Bill() {DetailBilId=Guid.Parse("d1d42909-a120-44b8-9fe0-ecc99d643168"),
                                            ManagerId=Guid.Parse("2cfe8617-8d05-46bd-8ad5-974488f1fe3c"),
                                            BillId=Guid.Parse("ef91c997-d758-44c6-8b9f-a66d7027e24c"),
                                            Dish=Dish.ICE_CREAM,
                                            Value=4.4, CreatedAt=new DateTime(2022,10,28) });
        detailBillList.Add(new Detail_Bill() {DetailBilId=Guid.Parse("d1d42909-a120-44b8-9fe0-ecc99d643169"),
                                            ManagerId=Guid.Parse("1cfe8617-8d05-46bd-8ad5-974488f1fe3c"),
                                            BillId=Guid.Parse("ef91c997-d758-44c6-8b9f-a66d7027e25c"),
                                            Dish=Dish.FRIED_CHICKEN,
                                            Value=4.4, CreatedAt=new DateTime(2022,10,28) });

        modelBuilder.Entity<Detail_Bill>( detailBill =>
        {
            detailBill.ToTable("Detail_Bill");
            detailBill.HasKey(p => p.DetailBilId);
            detailBill.Property(p => p.ManagerId).IsRequired();
            detailBill.Property(p => p.BillId).IsRequired();
            detailBill.Property(p => p.Dish).IsRequired();
            detailBill.Property(p => p.Value).IsRequired();
            detailBill.Property(p => p.CreatedAt);


            detailBill.HasOne(p => p.Manager).WithMany(p => p.Detail_Bills).HasForeignKey(p => p.ManagerId);
            detailBill.HasOne(p => p.Bill).WithMany(p => p.Detail_Bills).HasForeignKey(p => p.BillId);

            detailBill.HasData(detailBillList);
        });
    }
}
