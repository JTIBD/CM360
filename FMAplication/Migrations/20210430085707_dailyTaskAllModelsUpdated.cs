using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class dailyTaskAllModelsUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskStatus",
                table: "DailySurveyTasks");

            migrationBuilder.DropColumn(
                name: "TaskStatus",
                table: "DailyPosmTasks");

            migrationBuilder.DropColumn(
                name: "TaskStatus",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropColumn(
                name: "TaskStatus",
                table: "DailyAvTasks");

            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                table: "DailyTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "DailySurveyTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OutletId",
                table: "DailySurveyTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "DailyPosmTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OutletId",
                table: "DailyPosmTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "DailyCommunicationTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OutletId",
                table: "DailyCommunicationTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "DailyAvTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OutletId",
                table: "DailyAvTasks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                table: "DailyTasks");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "DailySurveyTasks");

            migrationBuilder.DropColumn(
                name: "OutletId",
                table: "DailySurveyTasks");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "DailyPosmTasks");

            migrationBuilder.DropColumn(
                name: "OutletId",
                table: "DailyPosmTasks");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropColumn(
                name: "OutletId",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "DailyAvTasks");

            migrationBuilder.DropColumn(
                name: "OutletId",
                table: "DailyAvTasks");

            migrationBuilder.AddColumn<int>(
                name: "TaskStatus",
                table: "DailySurveyTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatus",
                table: "DailyPosmTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatus",
                table: "DailyCommunicationTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatus",
                table: "DailyAvTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
