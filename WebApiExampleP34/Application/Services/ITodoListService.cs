using WebApiExampleP34.Infrastructure;
using WebApiExampleP34.Models;
using WebApiExampleP34.Models.DTO;

namespace WebApiExampleP34.Application.Services;

public interface ITodoListService
{
    public Task<IEnumerable<TodoListDto>> GetAllAsync();

    public Task<IEnumerable<TodoListDto>> GetAllForUserAsync(int userId);

    public Task<TodoListDto> CreateAsync(TodoListDto dto, int userId);

    public Task<TodoListDto> GetByIdAsync(int id);

    public Task<TodoListDto> UpdateAsync(int id, TodoListDto dto);

    public Task DeleteByIdAsync(int id);

    public Task<TodoItemDto> AddItemToListAsync(int listId, TodoItemDto itemDto);

    public Task<IEnumerable<TodoItemDto>> SearchItemsInListAsync(int listId, TodoItemSearchDto searchDto);
}
