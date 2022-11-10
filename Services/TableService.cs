using Microsoft.EntityFrameworkCore;
using ctds_webapi.Models;

namespace ctds_webapi.Services;

public class TableService : ITableService
{
    OracleDBContext context;

    public TableService(OracleDBContext dbContext)
    {
        context = dbContext;
    }

    public IEnumerable<Table> Get()
    {
        return context.Tables;
    }

    public async Task Save(Table table)
    {
        context.Tables.Add(table);

        await context.SaveChangesAsync();
    }

    public async Task Update(Guid id, Table table)
    {
        var managerToUpdate = context.Tables.Find(id);

        if(managerToUpdate != null)
        {
            managerToUpdate.Name = table.Name;
            managerToUpdate.Reserved = table.Reserved;
            managerToUpdate.Stalls = table.Stalls;

            await context.SaveChangesAsync();
        }
    }

    public async Task Delete(Guid id)
    {
        var managerToDelete = context.Tables.Find(id);

        if(managerToDelete !=null){
            context.Tables.Remove(managerToDelete);

            await context.SaveChangesAsync();
        }
    }
}

public interface ITableService
{
    IEnumerable<Table> Get();
    Task Save(Table table);
    Task Update(Guid id, Table table);
    Task Delete(Guid id);
}