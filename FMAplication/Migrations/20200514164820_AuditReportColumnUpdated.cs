using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class AuditReportColumnUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DailyAuditId",
                table: "AuditReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DailyAudits_DailyCMActivityId",
                table: "DailyAudits",
                column: "DailyCMActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditReports_DailyAuditId",
                table: "AuditReports",
                column: "DailyAuditId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditReports_DailyAudits_DailyAuditId",
                table: "AuditReports",
                column: "DailyAuditId",
                principalTable: "DailyAudits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyAudits_DailyCMActivities_DailyCMActivityId",
                table: "DailyAudits",
                column: "DailyCMActivityId",
                principalTable: "DailyCMActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditReports_DailyAudits_DailyAuditId",
                table: "AuditReports");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyAudits_DailyCMActivities_DailyCMActivityId",
                table: "DailyAudits");

            migrationBuilder.DropIndex(
                name: "IX_DailyAudits_DailyCMActivityId",
                table: "DailyAudits");

            migrationBuilder.DropIndex(
                name: "IX_AuditReports_DailyAuditId",
                table: "AuditReports");

            migrationBuilder.DropColumn(
                name: "DailyAuditId",
                table: "AuditReports");
        }
    }
}
