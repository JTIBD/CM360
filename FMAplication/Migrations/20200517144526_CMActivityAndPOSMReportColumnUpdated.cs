using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class CMActivityAndPOSMReportColumnUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAutid",
                table: "DailyCMActivities");

            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "POSMReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAudit",
                table: "DailyCMActivities",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "POSMReports");

            migrationBuilder.DropColumn(
                name: "IsAudit",
                table: "DailyCMActivities");

            migrationBuilder.AddColumn<bool>(
                name: "IsAutid",
                table: "DailyCMActivities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
