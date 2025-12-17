using WebApiExampleP34.Application;
using WebApiExampleP34.Application.Repositories;
using WebApiExampleP34.Infrastructure.Repositories;
using WebApiExampleP34.Models.DAL;

namespace WebApiExampleP34.Infrastructure;

public class UnitOfWork(TodoListContext context): IUnitOfWork
{
    private Lazy<ITodoItemRepository> _todoItems = new Lazy<ITodoItemRepository>(() => new TodoItemRepository(context));

    public ITodoItemRepository TodoItems => _todoItems.Value;

    public async Task SaveChangexAsync()
    {
        await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
