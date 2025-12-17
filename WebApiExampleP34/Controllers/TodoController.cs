using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using WebApiExampleP34.Application;
using WebApiExampleP34.Models;
using WebApiExampleP34.Models.Dto;

namespace WebApiExampleP34.Controllers;

[ApiController]
[Route("api/v1/todo")]
public class TodoController(IUnitOfWork unitOfWork) : ControllerBase
{

    [HttpGet("list")]
    [SwaggerOperation(
        Summary = "List all todo items",
        Description = "Retrieves a list of all todo items.",
        Tags = new[] { "Todo Operations" }
    )]
    [ProducesResponseType(typeof(IEnumerable<TodoItem>), 200)]
    public async Task<IEnumerable<TodoItem>> List()
    {
        return await unitOfWork.TodoItems.GetAll().ToListAsync();
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get a todo item by ID",
        Description = "Retrieves the details of a todo item identified by its ID.",
        Tags = new[] { "Todo Operations" }
    )]
    [ProducesResponseType(typeof(TodoItem), 200)]
    [ProducesResponseType(404)]
    public async Task<TodoItem> GetById([SwaggerParameter("Id of todo item", Required = true)] int id)
    {
        var item = await unitOfWork.TodoItems.GetByIdAsync(id);
        if (item == null)
        {
            Response.StatusCode = 404;
            return null!;
        }
        return item;
    }

    [HttpPost("{id}")]
    [SwaggerOperation(
        Summary = "Update a todo item by ID",
        Description = "Updates the details of a todo item identified by its ID.",
        OperationId = "UpdateTodoById",
        Tags = new[] { "Todo Operations" }
    )]
    [ProducesResponseType(typeof(OperationResult), 200)]
    [ProducesResponseType(typeof(OperationResult), StatusCodes.Status404NotFound)]
    public async Task<OperationResult> UpdateById([SwaggerRequestBody("Todo item content")][FromBody] TodoItem item)
    {
        var existingItem = await unitOfWork.TodoItems.GetByIdAsync(item.Id);
        if (existingItem == null)
        {
            Response.StatusCode = 404;
            return OperationResult.Fail("Item not found.");
        }

        existingItem.Title = item.Title;
        existingItem.Description = item.Description;
        existingItem.IsCompleted = item.IsCompleted;
        existingItem.Priority = item.Priority;

        await unitOfWork.TodoItems.UpdateAsync(existingItem);
        await unitOfWork.SaveChangexAsync();

        return OperationResult.Ok();
    }

    /// <summary>
    ///         
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    [SwaggerOperation(
        Tags = new[] { "Todo Operations" }
    )]
    [HttpPut("create")]
    public async Task<OperationResult> Create([FromBody] TodoItem item)
    {
        var newItem = new TodoItem
        {
            Title = item.Title,
            Description = item.Description,
            IsCompleted = item.IsCompleted,
            Priority = item.Priority,
            CreatedDate = DateTime.UtcNow
        };
        await unitOfWork.TodoItems.AddAsync(newItem);
        await unitOfWork.SaveChangexAsync();
        
        return OperationResult.Ok();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <responses code="200"></responses>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(OperationResult), 200)]
    [ProducesResponseType(typeof(OperationResult), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Tags = new[] { "Todo Operations" }
    )]
    public async Task<OperationResult> DeleteById(int id)
    {
        var existingItem = await unitOfWork.TodoItems.GetByIdAsync(id);
        if (existingItem == null)
        {
            Response.StatusCode = 404;
            return OperationResult.Fail("Item not found.");
        }
        // видалення з бази даних
        await unitOfWork.TodoItems.DeleteAsync(id);
        await unitOfWork.SaveChangexAsync();
        
        return OperationResult.Ok();
    }


}
