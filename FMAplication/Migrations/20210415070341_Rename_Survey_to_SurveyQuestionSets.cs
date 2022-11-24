using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Rename_Survey_to_SurveyQuestionSets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyReports_Surveys_SurveyId",
                table: "SurveyReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Surveys",
                table: "Surveys");

            migrationBuilder.RenameTable(
                name: "Surveys",
                newName: "SurveyQuestionSets");

            migrationBuilder.RenameIndex(
                name: "IX_Surveys_SurveyName",
                table: "SurveyQuestionSets",
                newName: "IX_SurveyQuestionSets_SurveyName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyQuestionSets",
                table: "SurveyQuestionSets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyReports_SurveyQuestionSets_SurveyId",
                table: "SurveyReports",
                column: "SurveyId",
                principalTable: "SurveyQuestionSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyReports_SurveyQuestionSets_SurveyId",
                table: "SurveyReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyQuestionSets",
                table: "SurveyQuestionSets");

            migrationBuilder.RenameTable(
                name: "SurveyQuestionSets",
                newName: "Surveys");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyQuestionSets_SurveyName",
                table: "Surveys",
                newName: "IX_Surveys_SurveyName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Surveys",
                table: "Surveys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyReports_Surveys_SurveyId",
                table: "SurveyReports",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
