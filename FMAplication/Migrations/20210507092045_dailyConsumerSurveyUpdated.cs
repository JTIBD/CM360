using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class dailyConsumerSurveyUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyConsumerSurveyTaskAnswers_DailySurveyTasks_DailySurveyTaskId",
                table: "DailyConsumerSurveyTaskAnswers");

            migrationBuilder.DropIndex(
                name: "IX_DailyConsumerSurveyTaskAnswers_DailySurveyTaskId",
                table: "DailyConsumerSurveyTaskAnswers");

            migrationBuilder.DropColumn(
                name: "DailySurveyTaskId",
                table: "DailyConsumerSurveyTaskAnswers");

            migrationBuilder.AddColumn<int>(
                name: "DailyConsumerSurveyTaskId",
                table: "DailyConsumerSurveyTaskAnswers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DailyConsumerSurveyTaskAnswers_DailyConsumerSurveyTaskId",
                table: "DailyConsumerSurveyTaskAnswers",
                column: "DailyConsumerSurveyTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyConsumerSurveyTaskAnswers_DailyConsumerSurveyTasks_DailyConsumerSurveyTaskId",
                table: "DailyConsumerSurveyTaskAnswers",
                column: "DailyConsumerSurveyTaskId",
                principalTable: "DailyConsumerSurveyTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyConsumerSurveyTaskAnswers_DailyConsumerSurveyTasks_DailyConsumerSurveyTaskId",
                table: "DailyConsumerSurveyTaskAnswers");

            migrationBuilder.DropIndex(
                name: "IX_DailyConsumerSurveyTaskAnswers_DailyConsumerSurveyTaskId",
                table: "DailyConsumerSurveyTaskAnswers");

            migrationBuilder.DropColumn(
                name: "DailyConsumerSurveyTaskId",
                table: "DailyConsumerSurveyTaskAnswers");

            migrationBuilder.AddColumn<int>(
                name: "DailySurveyTaskId",
                table: "DailyConsumerSurveyTaskAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DailyConsumerSurveyTaskAnswers_DailySurveyTaskId",
                table: "DailyConsumerSurveyTaskAnswers",
                column: "DailySurveyTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyConsumerSurveyTaskAnswers_DailySurveyTasks_DailySurveyTaskId",
                table: "DailyConsumerSurveyTaskAnswers",
                column: "DailySurveyTaskId",
                principalTable: "DailySurveyTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
