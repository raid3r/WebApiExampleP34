using WebApiExampleP34.Application.Repositories;
using WebApiExampleP34.Models;
using WebApiExampleP34.Models.DAL;

namespace WebApiExampleP34.Infrastructure.Repositories;

public class TodoListRepository : GenericRepository<TodoList>, ITodoListRepository
{
    public TodoListRepository(TodoListContext context): base(context)
    {

    }
}
