using WebApiExampleP34.Models.Constants;

namespace WebApiExampleP34.Models;

/// <summary>
/// Represents a to-do item with a title, description, completion status, and creation date.
/// </summary>
/// <remarks>The <see cref="TodoItem"/> class is typically used to model individual tasks or reminders in a to-do
/// list application.</remarks>
public class TodoItem
{
    /// <summary>
    /// Id of todo item
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Title of todo item
    /// </summary>
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public string Description { get; set; }
    public TodoItemPriority Priority { get; set; }
    public DateTime CreatedDate { get; set; }

}
