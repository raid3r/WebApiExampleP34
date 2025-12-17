namespace WebApiExampleP34.Models;

public class TodoList
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<TodoItem> Items { get; set; } = [];
}
