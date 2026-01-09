using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApiExampleP34.Application.Services;
using WebApiExampleP34.Models.DTO;

namespace WebApiExampleP34.Controllers;

[ApiController]
[Route("api/v1/todo")]
public class TodoController(ITodoItemService service) : ControllerBase
{
    [HttpGet("list")]
    [SwaggerOperation(
        Summary = "List all todo items",
        Description = "Retrieves a list of all todo items.",
        Tags = new[] { "Todo Operations" }
    )]
    [ProducesResponseType(typeof(IEnumerable<TodoItemDto>), 200)]
    public async Task<IEnumerable<TodoItemDto>> List()
    {
        return await service.GetAllAsync();
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get a todo item by ID",
        Description = "Retrieves the details of a todo item identified by its ID.",
        Tags = new[] { "Todo Operations" }
    )]
    [ProducesResponseType(typeof(TodoItemDto), 200)]
    [ProducesResponseType(404)]
    public async Task<TodoItemDto> GetById([SwaggerParameter("Id of todo item", Required = true)] int id)
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
    [ProducesResponseType(typeof(OperationResult<TodoItemDto>), 200)]
    [ProducesResponseType(typeof(OperationResult<TodoItemDto>), StatusCodes.Status404NotFound)]
    public async Task<OperationResult<TodoItemDto>> UpdateById(int id, [SwaggerRequestBody("Todo item content")][FromBody] TodoItemDto item)
    {
        try
        {
            return OperationResult<TodoItemDto>.Ok(await service.UpdateAsync(id, item));
        } catch (InvalidDataException)
        {
            Response.StatusCode = 404;
            return OperationResult<TodoItemDto>.Fail("Item not found.");
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
    public async Task<OperationResult<TodoItemDto>> Create([FromBody] TodoItemDto item)
    {
        return OperationResult<TodoItemDto>.Ok(await service.CreateAsync(item));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <responses code="200"></responses>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(OperationResult<TodoItemDto>), 200)]
    [ProducesResponseType(typeof(OperationResult<TodoItemDto>), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Tags = new[] { "Todo Operations" }
    )]
    public async Task<OperationResult<TodoItemDto>> DeleteById(int id)
    {
        try
        {
            await service.DeleteByIdAsync(id);
            return OperationResult<TodoItemDto>.Ok(null);
        }
        catch (Exception)
        {
            Response.StatusCode = 404;
            return OperationResult<TodoItemDto>.Fail("Item not found.");
        }
    }
}
