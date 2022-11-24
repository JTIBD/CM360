using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class removedFMuserFromCMUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CMUsers_UserInfos_FMUserId",
                table: "CMUsers");

            migrationBuilder.DropIndex(
                name: "IX_CMUsers_FMUserId",
                table: "CMUsers");

            migrationBuilder.DropColumn(
                name: "FMUserId",
                table: "CMUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "CMUsers",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "FMUserId",
                table: "CMUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CMUsers_FMUserId",
                table: "CMUsers",
                column: "FMUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CMUsers_UserInfos_FMUserId",
                table: "CMUsers",
                column: "FMUserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
