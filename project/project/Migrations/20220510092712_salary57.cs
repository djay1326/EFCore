using Microsoft.EntityFrameworkCore.Migrations;

namespace project.Migrations
{
    public partial class salary57 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "statuspos",
                table: "leave",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "statuspos",
                table: "leave");
        }
    }
}
