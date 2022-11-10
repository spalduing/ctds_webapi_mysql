using ctds_webapi;
using ctds_webapi.Services;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using AutoMapper;
using ctds_webapi.Configurations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
{
    var services = builder.Services;

    services.AddCors( o =>{
        o.AddPolicy("AllowAll", builder =>
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
        );
    });
    services.AddAutoMapper(typeof(MapperInitializer));
    // services.AddMvc();

    var connectionString = builder.Configuration["ConnectionStrings:WebApiDatabase"];

    services.AddDbContext<OracleDBContext>(
        dbContextOptions => dbContextOptions
            .UseOracle(connectionString)
    );
}

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IWaiterService, WaiterService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<IDetail_BillService, Detail_BillService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run("https://localhost:3000");
