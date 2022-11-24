using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class userinfoUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CMUsers_UserInfos_UserInfoId",
                table: "CMUsers");

            migrationBuilder.DropIndex(
                name: "IX_CMUsers_UserInfoId",
                table: "CMUsers");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "CMUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "CMUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CMUsers_UserInfoId",
                table: "CMUsers",
                column: "UserInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_CMUsers_UserInfos_UserInfoId",
                table: "CMUsers",
                column: "UserInfoId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
