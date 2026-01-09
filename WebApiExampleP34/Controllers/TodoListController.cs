using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebApiExampleP34.Application.Services;
using WebApiExampleP34.Models.DTO;

namespace WebApiExampleP34.Controllers;

[Authorize]
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
        var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        return await service.GetAllForUserAsync(int.Parse(userId));
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
            // TODO Проблема безпеки (користувач може отримати доступ до чужого списку)
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
    [ProducesResponseType(typeof(OperationResult<TodoListDto>), 200)]
    [ProducesResponseType(typeof(OperationResult<TodoListDto>), StatusCodes.Status404NotFound)]
    public async Task<OperationResult<TodoListDto>> UpdateById(int id, [SwaggerRequestBody("Todo item content")][FromBody] TodoListDto item)
    {
        try
        {
            // TODO Проблема безпеки (користувач може отримати доступ до чужого списку)
            ;
            return OperationResult<TodoListDto>.Ok(await service.UpdateAsync(id, item));
        }
        catch (InvalidDataException)
        {
            Response.StatusCode = 404;
            return OperationResult<TodoListDto>.Fail("Item not found.");
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
    public async Task<OperationResult<TodoListDto>> Create([FromBody] TodoListDto item)
    {
        var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        return OperationResult<TodoListDto>.Ok(await service.CreateAsync(item, int.Parse(userId)));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <responses code="200"></responses>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(OperationResult<TodoListDto>), 200)]
    [ProducesResponseType(typeof(OperationResult<TodoListDto>), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Tags = new[] { "Todo Operations" }
    )]
    public async Task<OperationResult<TodoListDto>> DeleteById(int id)
    {
        try
        {
            await service.DeleteByIdAsync(id);
            return OperationResult<TodoListDto>.Ok(null);
        }
        catch (Exception)
        {
            Response.StatusCode = 404;
            return OperationResult<TodoListDto>.Fail("Item not found.");
        }
    }


    [HttpPost("{listId}/add-item")]
    public async Task<OperationResult<TodoItemDto>> AddItemToList(int listId, TodoItemDto item)
    {
        try
        {
            return OperationResult<TodoItemDto>.Ok(await service.AddItemToListAsync(listId, item));
        }
        catch (Exception)
        {
            Response.StatusCode = 404;
            return OperationResult<TodoItemDto>.Fail("List not found.");
        }
    }

    // /api/v1/todo-list/search/1?
    [HttpGet("search/{listId}")]
    public async Task<IEnumerable<TodoItemDto>> Search(int listId, [FromQuery] TodoItemSearchDto search)
    {
        return await service.SearchItemsInListAsync(listId, search);
    }

}
