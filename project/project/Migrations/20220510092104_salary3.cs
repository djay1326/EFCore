using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace project.Migrations
{
    public partial class salary3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "statuspos",
                table: "leave");

            migrationBuilder.CreateTable(
                name: "salary",
                columns: table => new
                {
                    salaryid = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    basic = table.Column<int>(nullable: true),
                    tax = table.Column<int>(nullable: true),
                    final = table.Column<int>(nullable: true),
                    createddate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salary", x => x.salaryid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salary");

            migrationBuilder.AddColumn<int>(
                name: "statuspos",
                table: "leave",
                type: "int",
                nullable: true);
        }
    }
}
