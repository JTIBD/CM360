using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class CheckIncheckOutDateTimeAddedInDailyTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckIn",
                table: "DailySurveyTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOut",
                table: "DailySurveyTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckIn",
                table: "DailyPosmTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOut",
                table: "DailyPosmTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckIn",
                table: "DailyInformationTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOut",
                table: "DailyInformationTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckIn",
                table: "DailyConsumerSurveyTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOut",
                table: "DailyConsumerSurveyTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckIn",
                table: "DailyCommunicationTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOut",
                table: "DailyCommunicationTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckIn",
                table: "DailyAvTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOut",
                table: "DailyAvTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckIn",
                table: "DailyAuditTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOut",
                table: "DailyAuditTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "DailySurveyTasks");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "DailySurveyTasks");

            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "DailyPosmTasks");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "DailyPosmTasks");

            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "DailyInformationTasks");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "DailyInformationTasks");

            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "DailyConsumerSurveyTasks");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "DailyConsumerSurveyTasks");

            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "DailyAvTasks");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "DailyAvTasks");

            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "DailyAuditTasks");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "DailyAuditTasks");
        }
    }
}
