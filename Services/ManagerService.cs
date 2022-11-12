using Microsoft.EntityFrameworkCore;
using ctds_webapi.Models;

namespace ctds_webapi.Services;

public class ManagerService : IManagerService
{
    OracleDBContext context;

    public ManagerService(OracleDBContext dbContext)
    {
        context = dbContext;
    }

    private void throwNotFoundException(Manager manager, Guid id)
    {
        if (manager == null)
        {
            Exception ex = new Exception($"Manager with id:{id} cannot be found");
            throw ex;
        }
    }

    public IEnumerable<Manager> Get()
    {
        return context.Managers;
    }

    public Manager GetById(Guid id)
    {
            Manager manager = context.Managers.Find(id);
            throwNotFoundException(manager, id);

            return manager;
    }
    public async Task Save(Manager manager)
    {
        context.Managers.Add(manager);

        await context.SaveChangesAsync();
    }

    public async Task Update(Guid id, Manager manager)
    {
        var managerToUpdate = context.Managers.Find(id);

        if (managerToUpdate != null)
        {
            managerToUpdate.Name = manager.Name;
            managerToUpdate.LastName = manager.LastName;
            managerToUpdate.Age = manager.Age;
            managerToUpdate.Seniority = manager.Seniority;

            await context.SaveChangesAsync();
        }
    }

    public async Task Delete(Guid id)
    {
        var managerToDelete = context.Managers.Find(id);

        if (managerToDelete != null)
        {
            context.Managers.Remove(managerToDelete);

            await context.SaveChangesAsync();
        }
    }
}

public interface IManagerService
{
    IEnumerable<Manager> Get();
    Manager GetById(Guid id);
    Task Save(Manager manager);
    Task Update(Guid id, Manager manager);
    Task Delete(Guid id);
}