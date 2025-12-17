using WebApiExampleP34.Infrastructure;
using WebApiExampleP34.Models;
using WebApiExampleP34.Models.DTO;

namespace WebApiExampleP34.Application.Services;

public interface ITodoItemService
{
    public Task<IEnumerable<TodoItemDto>> GetAllAsync();

    public Task CreateAsync(TodoItemDto dto);

    public Task<TodoItemDto> GetByIdAsync(int id);

    public Task UpdateAsync(int id, TodoItemDto dto);

    public Task DeleteByIdAsync(int id);
}
