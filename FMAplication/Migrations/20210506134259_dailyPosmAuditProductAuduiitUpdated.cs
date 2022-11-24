using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class dailyPosmAuditProductAuduiitUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Result",
                table: "DailyProductsAuditTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActionType",
                table: "DailyPosmAuditTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Result",
                table: "DailyPosmAuditTasks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "DailyProductsAuditTasks");

            migrationBuilder.DropColumn(
                name: "ActionType",
                table: "DailyPosmAuditTasks");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "DailyPosmAuditTasks");
        }
    }
}
