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

    private void throwNotFoundException(Table table, Guid id)
    {
        if (table == null)
        {
            Exception ex = new Exception($"Table with id:{id} cannot be found");
            throw ex;
        }
    }

    public IEnumerable<Table> Get()
    {
        return context.Tables;
    }

    public Table GetById(Guid id)
    {
            Table table = context.Tables.Find(id);
            throwNotFoundException(table, id);

            return table;
    }

    public async Task Save(Table table)
    {
        table.TableId = Guid.NewGuid();
        context.Tables.Add(table);

        await context.SaveChangesAsync();
    }

    public async Task Update(Guid id, Table table)
    {
        var tableToUpdate = context.Tables.Find(id);
        throwNotFoundException(tableToUpdate, id);

        tableToUpdate.Name = table.Name;
        tableToUpdate.Reserved = table.Reserved;
        tableToUpdate.Stalls = table.Stalls;

        await context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var tableToDelete = context.Tables.Find(id);
        throwNotFoundException(tableToDelete, id);
        context.Tables.Remove(tableToDelete);

        await context.SaveChangesAsync();
    }
}

public interface ITableService
{
    IEnumerable<Table> Get();
    Table GetById(Guid id);
    Task Save(Table table);
    Task Update(Guid id, Table table);
    Task Delete(Guid id);
}