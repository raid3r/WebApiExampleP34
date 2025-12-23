using WebApiExampleP34.Application;
using WebApiExampleP34.Application.Repositories;
using WebApiExampleP34.Infrastructure.Repositories;
using WebApiExampleP34.Models.DAL;

namespace WebApiExampleP34.Infrastructure;

public class UnitOfWork(TodoListContext context): IUnitOfWork
{
    private readonly Lazy<ITodoItemRepository> _todoItems = new(() => new TodoItemRepository(context));
    private readonly Lazy<ITodoListRepository> _todoLists = new(() => new TodoListRepository(context));
    private readonly Lazy<IUserRepository> _users = new(() => new UserRepository(context));

    public ITodoItemRepository TodoItems => _todoItems.Value;
    public ITodoListRepository TodoLists => _todoLists.Value;
    public IUserRepository Users => _users.Value;

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
