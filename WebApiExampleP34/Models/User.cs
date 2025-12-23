using Microsoft.AspNetCore.Identity;

namespace WebApiExampleP34.Models;

public class User: IdentityUser<int>
{
    public virtual ICollection<TodoList> TodoLists { get; set; } = [];
}
