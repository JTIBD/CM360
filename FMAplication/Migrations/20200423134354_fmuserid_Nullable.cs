using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class fmuserid_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CMUsers_UserInfos_FMUserId",
                table: "CMUsers");

            migrationBuilder.AlterColumn<int>(
                name: "FMUserId",
                table: "CMUsers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CMUsers_UserInfos_FMUserId",
                table: "CMUsers",
                column: "FMUserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CMUsers_UserInfos_FMUserId",
                table: "CMUsers");

            migrationBuilder.AlterColumn<int>(
                name: "FMUserId",
                table: "CMUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CMUsers_UserInfos_FMUserId",
                table: "CMUsers",
                column: "FMUserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
