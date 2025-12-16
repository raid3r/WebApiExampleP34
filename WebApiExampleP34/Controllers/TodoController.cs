using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApiExampleP34.Models;
using WebApiExampleP34.Models.Dto;

namespace WebApiExampleP34.Controllers;

[ApiController]
[Route("api/v1/todo")]
public class TodoController : ControllerBase
{

    [HttpGet("list")]
    [SwaggerOperation(
        Summary = "List all todo items",
        Description = "Retrieves a list of all todo items.",
        Tags = new[] { "Todo Operations" }
    )]
    [ProducesResponseType(typeof(IEnumerable<TodoItem>), 200)]
    public IEnumerable<TodoItem> List()
    {
        return new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Buy groceries", IsCompleted = false, Description = "Milk, Bread, Eggs", CreatedDate = DateTime.Now },
            new TodoItem { Id = 2, Title = "Walk the dog", IsCompleted = true, Description = "Evening walk in the park", CreatedDate = DateTime.Now.AddDays(-1) },
            new TodoItem { Id = 3, Title = "Read a book", IsCompleted = false, Description = "Finish reading 'The Great Gatsby'", CreatedDate = DateTime.Now.AddDays(-2) }
        };
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get a todo item by ID",
        Description = "Retrieves the details of a todo item identified by its ID.",
        Tags = new[] { "Todo Operations" }
    )]
    [ProducesResponseType(typeof(TodoItem), 200)]
    public TodoItem GetById([SwaggerParameter("Id of todo item", Required = true)] int id)
    {
        var todoItem = new TodoItem { Id = id, Title = "Sample Task", IsCompleted = false, Description = "This is a sample task", CreatedDate = DateTime.Now };
        return todoItem;
    }

    [HttpPost("{id}")]
    [SwaggerOperation(
        Summary = "Update a todo item by ID",
        Description = "Updates the details of a todo item identified by its ID.",
        OperationId = "UpdateTodoById",
        Tags = new[] { "Todo Operations" }
    )]
    [ProducesResponseType(typeof(OperationResult), 200)]
    [ProducesResponseType(typeof(OperationResult), 500)]
    public OperationResult UpdateById([SwaggerRequestBody("Todo item content")][FromBody] TodoItem item)
    {
        
        Response.StatusCode = 500;
        return OperationResult.Fail("An error occurred while updating the item.");


        // валідація та збереження в базу даних
       //return OperationResult.Ok();
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
    public OperationResult Create([FromBody] TodoItem item)
    {
        // додавання в базу даних
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
    [ProducesResponseType(typeof(OperationResult), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Tags = new[] { "Todo Operations" }
    )]
    public OperationResult DeleteById(int id)
    {
        // видалення з бази даних
        return OperationResult.Ok();
    }


}
