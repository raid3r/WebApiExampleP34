using WebApiExampleP34.Application.Repositories;

namespace WebApiExampleP34.Application;

public interface IUnitOfWork: IDisposable
{
    Task SaveChangesAsync();

    ITodoItemRepository TodoItems { get; }
    ITodoListRepository TodoLists { get; }
}
