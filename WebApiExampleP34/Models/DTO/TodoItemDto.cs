using WebApiExampleP34.Models.Constants;

namespace WebApiExampleP34.Models.DTO;

public class TodoItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public string Description { get; set; } = string.Empty;
    public TodoItemPriority Priority { get; set; }
}
