using Microsoft.EntityFrameworkCore.Migrations;

namespace project.Migrations
{
    public partial class userids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userid",
                table: "salary",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_salary_userid",
                table: "salary",
                column: "userid");

            migrationBuilder.AddForeignKey(
                name: "FK_salary_AspNetUsers_userid",
                table: "salary",
                column: "userid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salary_AspNetUsers_userid",
                table: "salary");

            migrationBuilder.DropIndex(
                name: "IX_salary_userid",
                table: "salary");

            migrationBuilder.DropColumn(
                name: "userid",
                table: "salary");
        }
    }
}
