using WebApiExampleP34.Infrastructure;
using WebApiExampleP34.Models;
using WebApiExampleP34.Models.DTO;

namespace WebApiExampleP34.Application.Services;

public interface ITodoListService
{
    public Task<IEnumerable<TodoListDto>> GetAllAsync();

    public Task CreateAsync(TodoListDto dto);

    public Task<TodoListDto> GetByIdAsync(int id);

    public Task UpdateAsync(int id, TodoListDto dto);

    public Task DeleteByIdAsync(int id);

    public Task AddItemToListAsync(int listId, TodoItemDto itemDto);

    public Task<IEnumerable<TodoItemDto>> SearchItemsInListAsync(int listId, TodoItemSearchDto searchDto);
}
