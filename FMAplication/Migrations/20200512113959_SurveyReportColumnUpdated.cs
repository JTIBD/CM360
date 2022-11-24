using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class SurveyReportColumnUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyActivityId",
                table: "SurveyReports");

            migrationBuilder.AddColumn<int>(
                name: "DailyCMActivityId",
                table: "SurveyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyReports_DailyCMActivityId",
                table: "SurveyReports",
                column: "DailyCMActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPOSMs_DailyCMActivityId",
                table: "DailyPOSMs",
                column: "DailyCMActivityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyPOSMs_DailyCMActivities_DailyCMActivityId",
                table: "DailyPOSMs",
                column: "DailyCMActivityId",
                principalTable: "DailyCMActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyReports_DailyCMActivities_DailyCMActivityId",
                table: "SurveyReports",
                column: "DailyCMActivityId",
                principalTable: "DailyCMActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyPOSMs_DailyCMActivities_DailyCMActivityId",
                table: "DailyPOSMs");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyReports_DailyCMActivities_DailyCMActivityId",
                table: "SurveyReports");

            migrationBuilder.DropIndex(
                name: "IX_SurveyReports_DailyCMActivityId",
                table: "SurveyReports");

            migrationBuilder.DropIndex(
                name: "IX_DailyPOSMs_DailyCMActivityId",
                table: "DailyPOSMs");

            migrationBuilder.DropColumn(
                name: "DailyCMActivityId",
                table: "SurveyReports");

            migrationBuilder.AddColumn<int>(
                name: "DailyActivityId",
                table: "SurveyReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
