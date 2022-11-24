using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class IsOutletOpen_Column_of_tasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOutletOpen",
                table: "DailySurveyTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOutletOpen",
                table: "DailyPosmTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOutletOpen",
                table: "DailyInformationTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOutletOpen",
                table: "DailyConsumerSurveyTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOutletOpen",
                table: "DailyCommunicationTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOutletOpen",
                table: "DailyAvTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOutletOpen",
                table: "DailyAuditTasks",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOutletOpen",
                table: "DailySurveyTasks");

            migrationBuilder.DropColumn(
                name: "IsOutletOpen",
                table: "DailyPosmTasks");

            migrationBuilder.DropColumn(
                name: "IsOutletOpen",
                table: "DailyInformationTasks");

            migrationBuilder.DropColumn(
                name: "IsOutletOpen",
                table: "DailyConsumerSurveyTasks");

            migrationBuilder.DropColumn(
                name: "IsOutletOpen",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropColumn(
                name: "IsOutletOpen",
                table: "DailyAvTasks");

            migrationBuilder.DropColumn(
                name: "IsOutletOpen",
                table: "DailyAuditTasks");
        }
    }
}
