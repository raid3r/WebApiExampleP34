using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApiExampleP34.Models.DAL;

public class TodoListContext : DbContext
{
    // Конструктор за замовченням
    public TodoListContext() : base() { }

    // Конструктор з параметрами для налаштування контексту
    public TodoListContext(DbContextOptions<TodoListContext> options) : base(options) { }

    // Визначення DbSet для сутностей
    public DbSet<TodoItem> TodoItems { get; set; }

    // Метод для налаштування моделі та конфігурації бази даних
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=SILVERSTONE\\SQLEXPRESS;Initial Catalog=WebApiExampleP34;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Connect Timeout=60;Encrypt=True;Trust Server Certificate=True;");

        }
        // Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False

    }
}
