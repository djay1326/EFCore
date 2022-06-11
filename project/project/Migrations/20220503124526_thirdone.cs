using Microsoft.EntityFrameworkCore.Migrations;

namespace project.Migrations
{
    public partial class thirdone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leave_AspNetUsers_userid",
                table: "leave");

            migrationBuilder.AlterColumn<int>(
                name: "userid",
                table: "leave",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_leave_AspNetUsers_userid",
                table: "leave",
                column: "userid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leave_AspNetUsers_userid",
                table: "leave");

            migrationBuilder.AlterColumn<int>(
                name: "userid",
                table: "leave",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_leave_AspNetUsers_userid",
                table: "leave",
                column: "userid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
