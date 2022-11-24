using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class SurveyReportColumnSurveyTypeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConsumerSurvey",
                table: "SurveyReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyReports_QuestionId",
                table: "SurveyReports",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyReports_SurveyId",
                table: "SurveyReports",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestionCollections_Questions_QuestionId",
                table: "SurveyQuestionCollections",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyReports_Questions_QuestionId",
                table: "SurveyReports",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyReports_Surveys_SurveyId",
                table: "SurveyReports",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestionCollections_Questions_QuestionId",
                table: "SurveyQuestionCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyReports_Questions_QuestionId",
                table: "SurveyReports");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyReports_Surveys_SurveyId",
                table: "SurveyReports");

            migrationBuilder.DropIndex(
                name: "IX_SurveyReports_QuestionId",
                table: "SurveyReports");

            migrationBuilder.DropIndex(
                name: "IX_SurveyReports_SurveyId",
                table: "SurveyReports");

            migrationBuilder.DropColumn(
                name: "IsConsumerSurvey",
                table: "SurveyReports");
        }
    }
}
