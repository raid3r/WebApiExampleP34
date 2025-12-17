using WebApiExampleP34.Application.Repositories;

namespace WebApiExampleP34.Application;

public interface IUnitOfWork: IDisposable
{
    Task SaveChangexAsync();

    public ITodoItemRepository TodoItems { get; }
}
