
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

    public async Task<IEnumerable<TodoListDto>> GetAllForUserAsync(int userId)
    {
        return await unitOfWork.TodoLists
           .GetAll()
           .Include(x => x.User)
           .Where(x => x.User.Id == userId)
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


    public async Task CreateAsync(TodoListDto dto, int userId)
    {
        var model = new TodoList
        {
            Name = dto.Name,
            User = await unitOfWork.Users.GetAll().FirstAsync(x => x.Id == userId)
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

    public async Task<IEnumerable<TodoItemDto>> SearchItemsInListAsync(int listId, TodoItemSearchDto search)
    {
        // Створили запит, який отримує всі елементи завдань для певного списку
        var query = unitOfWork
            .TodoItems
            .GetAll()
            .Include(x => x.TodoList)
            .Where(x => x.TodoList.Id == listId);


        // Динамічно додаємо фільтри до запиту на основі параметрів пошуку
        if (search.TitleContains is not null)
        {
            query = query.Where(x => x.Title.Contains(search.TitleContains));
        }

        if (search.IsCompleted is not null)
        {
            query = query.Where(x => x.IsCompleted == search.IsCompleted);
        }

        if (search.DescriptionContains is not null)
        {
            query = query.Where(x => x.Description.Contains(search.DescriptionContains));
        }

        if (search.Priorities is not null && search.Priorities.Any())
        {
            query = query.Where(x => search.Priorities!.Contains(x.Priority));
        }

        // Виконуємо запит і проєктуємо результати у DTO
        return await query
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
}

