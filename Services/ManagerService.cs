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

    public IEnumerable<Manager> Get()
    {
        return context.Managers;
    }

    public async Task Save(Manager manager)
    {
        context.Managers.Add(manager);

        await context.SaveChangesAsync();
    }

    public async Task Update(Guid id, Manager manager)
    {
        var managerToUpdate = context.Managers.Find(id);

        if(managerToUpdate != null)
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

        if(managerToDelete !=null){
            context.Managers.Remove(managerToDelete);

            await context.SaveChangesAsync();
        }
    }
}

public interface IManagerService
{
    IEnumerable<Manager> Get();
    Task Save(Manager manager);
    Task Update(Guid id, Manager manager);
    Task Delete(Guid id);
}