using Microsoft.EntityFrameworkCore.Migrations;

namespace project.Migrations
{
    public partial class foreignone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_leave_userid",
                table: "leave",
                column: "userid");

            migrationBuilder.AddForeignKey(
                name: "FK_leave_AspNetUsers_userid",
                table: "leave",
                column: "userid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leave_AspNetUsers_userid",
                table: "leave");

            migrationBuilder.DropIndex(
                name: "IX_leave_userid",
                table: "leave");
        }
    }
}
