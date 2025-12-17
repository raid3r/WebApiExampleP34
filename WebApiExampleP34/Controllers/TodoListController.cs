using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApiExampleP34.Application.Services;
using WebApiExampleP34.Models.DTO;

namespace WebApiExampleP34.Controllers;

[ApiController]
[Route("api/v1/todo-list")]
public class TodoListController(ITodoListService service) : ControllerBase
{
    [HttpGet("list")]
    [SwaggerOperation(
        Summary = "List all todo items",
        Description = "Retrieves a list of all todo items.",
        Tags = new[] { "Todo Operations" }
    )]
    [ProducesResponseType(typeof(IEnumerable<TodoListDto>), 200)]
    public async Task<IEnumerable<TodoListDto>> List()
    {
        return await service.GetAllAsync();
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get a todo item by ID",
        Description = "Retrieves the details of a todo item identified by its ID.",
        Tags = new[] { "Todo Operations" }
    )]
    [ProducesResponseType(typeof(TodoListDto), 200)]
    [ProducesResponseType(404)]
    public async Task<TodoListDto> GetById([SwaggerParameter("Id of todo item", Required = true)] int id)
    {
        try
        {
            return await service.GetByIdAsync(id);
        }
        catch (Exception)
        {
            Response.StatusCode = 404;
            return null!;
        }
        
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
    public async Task<OperationResult> UpdateById(int id, [SwaggerRequestBody("Todo item content")][FromBody] TodoListDto item)
    {
        try
        {
            await service.UpdateAsync(id, item);
            return OperationResult.Ok();
        } catch (InvalidDataException)
        {
            Response.StatusCode = 404;
            return OperationResult.Fail("Item not found.");
        }
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
    public async Task<OperationResult> Create([FromBody] TodoListDto item)
    {
        await service.CreateAsync(item);
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
        try
        {
            await service.DeleteByIdAsync(id);
            return OperationResult.Ok();
        }
        catch (Exception)
        {
            Response.StatusCode = 404;
            return OperationResult.Fail("Item not found.");
        }
    }


    [HttpPost("{listId}/add-item")]
    public async Task<OperationResult> AddItemToList(int listId, TodoItemDto item)
    {
        try
        {
            await service.AddItemToListAsync(listId, item);
            return OperationResult.Ok();
        }
        catch (Exception)
        {
            Response.StatusCode = 404;
            return OperationResult.Fail("List not found.");
        }
    }

}
