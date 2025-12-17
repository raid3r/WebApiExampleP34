using WebApiExampleP34.Application.Repositories;
using WebApiExampleP34.Models;
using WebApiExampleP34.Models.DAL;

namespace WebApiExampleP34.Infrastructure.Repositories;

public class TodoItemRepository: GenericRepository<TodoItem>, ITodoItemRepository
{
    public TodoItemRepository(TodoListContext context): base(context)
    {

    }
}
