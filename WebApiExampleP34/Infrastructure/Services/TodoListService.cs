
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApiExampleP34.Application;
using WebApiExampleP34.Application.Services;
using WebApiExampleP34.Models;
using WebApiExampleP34.Models.DTO;

namespace WebApiExampleP34.Infrastructure.Services;

public class TodoListService(IUnitOfWork unitOfWork) : ITodoListService
{
    public async Task<IEnumerable<TodoListDto>> GetAllAsync()
    {
        return await unitOfWork.TodoLists
           .GetAll()
           .Select(x => new TodoListDto
           {
               Id = x.Id,
               Name = x.Name,
               Items = x.Items.Select(item => new TodoItemDto
               {
                   Id = item.Id,
                   Title = item.Title,
                   Description = item.Description,
                   IsCompleted = item.IsCompleted,
                   Priority = item.Priority
               }).ToList()
           })
           .ToListAsync();
    }


    public async Task CreateAsync(TodoListDto dto)
    {
        var model = new TodoList
        {
            Name = dto.Name,
        };
        await unitOfWork.TodoLists.AddAsync(model);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<TodoListDto> GetByIdAsync(int id)
    {
        var item = await unitOfWork.TodoLists
            .GetAll()
            .Include(x => x.Items)
            .Select(x => new TodoListDto
            {
                Id = x.Id,
                Name = x.Name,
                Items = x.Items.Select(item => new TodoItemDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    IsCompleted = item.IsCompleted,
                    Priority = item.Priority
                }).ToList()
            })
            .FirstAsync(x => x.Id == id);
        return item;
    }

    public async Task UpdateAsync(int id, TodoListDto dto)
    {
        var item = await unitOfWork.TodoLists.GetAll().FirstAsync(x => x.Id == id);
        item.Name = dto.Name;

        await unitOfWork.TodoLists.UpdateAsync(item);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var item = await unitOfWork.TodoLists.GetAll().FirstAsync(x => x.Id == id);
        await unitOfWork.TodoItems.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task AddItemToListAsync(int listId, TodoItemDto itemDto)
    {
        var list = await unitOfWork.TodoLists.GetAll()
            .Include(x => x.Items)
            .FirstAsync(x => x.Id == listId);
        var todoItem = new TodoItem
        {
            Title = itemDto.Title,
            Description = itemDto.Description,
            IsCompleted = itemDto.IsCompleted,
            Priority = itemDto.Priority
        };
        list.Items.Add(todoItem);
        await unitOfWork.SaveChangesAsync();
    }
}

