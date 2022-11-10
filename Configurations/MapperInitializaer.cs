using AutoMapper;
using ctds_webapi.Models;

namespace ctds_webapi.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
                CreateMap<Waiter, WaiterDTO>().ReverseMap();
                CreateMap<Waiter, CreateWaiterDTO>().ReverseMap();

                CreateMap<Customer, CustomerDTO>().ReverseMap();
                CreateMap<Customer, CreateCustomerDTO>().ReverseMap();

                CreateMap<Table, TableDTO>().ReverseMap();
                CreateMap<Table, CreateTableDTO>().ReverseMap();

                CreateMap<Manager, ManagerDTO>().ReverseMap();
                CreateMap<Manager, CreateManagerDTO>().ReverseMap();

                CreateMap<Bill, BillDTO>().ReverseMap();
                CreateMap<Bill, CreateBillDTO>().ReverseMap();

                CreateMap<Detail_Bill, Detail_BillDTO>().ReverseMap();
                CreateMap<Detail_Bill, CreateDetail_BillDTO>().ReverseMap();
        }
    }
}