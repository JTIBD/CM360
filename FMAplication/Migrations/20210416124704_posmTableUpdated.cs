using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class posmTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDownloaded",
                table: "PosmTaskAssigns");

            migrationBuilder.AddColumn<int>(
                name: "TaskStatus",
                table: "PosmTaskAssigns",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskStatus",
                table: "PosmTaskAssigns");

            migrationBuilder.AddColumn<bool>(
                name: "IsDownloaded",
                table: "PosmTaskAssigns",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
