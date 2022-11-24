using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class POSMReportColumnAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DailyPOSMId",
                table: "POSMReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_POSMReports_DailyPOSMId",
                table: "POSMReports",
                column: "DailyPOSMId");

            migrationBuilder.AddForeignKey(
                name: "FK_POSMReports_DailyPOSMs_DailyPOSMId",
                table: "POSMReports",
                column: "DailyPOSMId",
                principalTable: "DailyPOSMs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POSMReports_DailyPOSMs_DailyPOSMId",
                table: "POSMReports");

            migrationBuilder.DropIndex(
                name: "IX_POSMReports_DailyPOSMId",
                table: "POSMReports");

            migrationBuilder.DropColumn(
                name: "DailyPOSMId",
                table: "POSMReports");
        }
    }
}
