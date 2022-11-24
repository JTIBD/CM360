using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class UserInfoTableModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AreaNodeId",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NationalNodeId",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionNodeId",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TerritoryNodeId",
                table: "UserInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaNodeId",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "NationalNodeId",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "RegionNodeId",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "TerritoryNodeId",
                table: "UserInfos");
        }
    }
}
