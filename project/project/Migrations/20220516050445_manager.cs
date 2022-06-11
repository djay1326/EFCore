using Microsoft.EntityFrameworkCore.Migrations;

namespace project.Migrations
{
    public partial class manager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "manager",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "manager",
                table: "AspNetUsers");
        }
    }
}
