using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class userinfoupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdGuid",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Groups",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "UserInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdGuid",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Groups",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "email",
                table: "UserInfos");
        }
    }
}
