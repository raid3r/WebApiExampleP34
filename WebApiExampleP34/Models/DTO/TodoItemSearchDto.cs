using WebApiExampleP34.Models.Constants;

namespace WebApiExampleP34.Models.DTO;

public class TodoItemSearchDto
{
    public string? TitleContains { get; set; }
    public bool? IsCompleted { get; set; }
    public string? DescriptionContains { get; set; }
    public List<TodoItemPriority>? Priorities { get;set; }
}
