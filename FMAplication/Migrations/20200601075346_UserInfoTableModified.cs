using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class UserInfoTableModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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


            migrationBuilder.AddColumn<int>(
                name: "HierarchyId",
                table: "UserInfos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HierarchyId",
                table: "UserInfos");


            migrationBuilder.AddColumn<int>(
                name: "AreaNodeId",
                table: "UserInfos",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NationalNodeId",
                table: "UserInfos",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegionNodeId",
                table: "UserInfos",
                nullable: true,
                defaultValue: 0);


        }
    }
}
