using WebApiExampleP34.Models.Constants;

namespace WebApiExampleP34.Models.DTO;

public class TodoListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<TodoItemDto> Items { get; set; } = [];
}
