using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApiExampleP34.Application;
using WebApiExampleP34.Application.Services;
using WebApiExampleP34.Models;
using WebApiExampleP34.Models.DTO;

namespace WebApiExampleP34.Infrastructure.Services;

public class TodoItemService(IUnitOfWork unitOfWork) : ITodoItemService
{
    public async Task<IEnumerable<TodoItemDto>> GetAllAsync()
    {
        return await unitOfWork.TodoItems
           .GetAll()
           .Select(x => new TodoItemDto
           {
               Id = x.Id,
               Title = x.Title,
               Description = x.Description,
               IsCompleted = x.IsCompleted,
               Priority = x.Priority
           })
           .ToListAsync();
    }


    public async Task CreateAsync(TodoItemDto dto)
    {
        var todoItem = new TodoItem
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted,
            Priority = dto.Priority
        };
        await unitOfWork.TodoItems.AddAsync(todoItem);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<TodoItemDto> GetByIdAsync(int id)
    {
        var item = await unitOfWork.TodoItems
            .GetAll()
            .Select(x => new TodoItemDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                IsCompleted = x.IsCompleted,
                Priority = x.Priority
            }).FirstAsync(x => x.Id == id);
        return item;
    }

    public async Task UpdateAsync(int id, TodoItemDto dto)
    {
        var item = await unitOfWork.TodoItems.GetAll().FirstAsync(x => x.Id == id);
        item.Title = dto.Title;
        item.Description = dto.Description;
        item.IsCompleted = dto.IsCompleted;
        item.Priority = dto.Priority;

        await unitOfWork.TodoItems.UpdateAsync(item);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var item = await unitOfWork.TodoItems.GetAll().FirstAsync(x => x.Id == id);
        await unitOfWork.TodoItems.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
}


