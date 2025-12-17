using Microsoft.EntityFrameworkCore;
using WebApiExampleP34.Models.DAL;

namespace WebApiExampleP34.Application.Repositories;

public class GenericRepository<T>(TodoListContext context) 
    : IGenericRepository<T> where T : class
{
    public IQueryable<T> GetAll()
    {
        return context.Set<T>().AsQueryable();
    }
    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }
    public async Task AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        
    }
    public async Task UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            context.Set<T>().Remove(entity);
        }
    }   
}
