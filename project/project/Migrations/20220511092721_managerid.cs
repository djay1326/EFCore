using Microsoft.EntityFrameworkCore.Migrations;

namespace project.Migrations
{
    public partial class managerid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "managerid",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "managerid",
                table: "AspNetUsers");
        }
    }
}
