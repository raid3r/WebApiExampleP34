using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiExampleP34.Migrations
{
    /// <inheritdoc />
    public partial class TodoItemPriority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "TodoItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "TodoItems");
        }
    }
}
