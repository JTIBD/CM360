using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Rename_SurveyName_to_Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SurveyName",
                table: "SurveyQuestionSets",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyQuestionSets_SurveyName",
                table: "SurveyQuestionSets",
                newName: "IX_SurveyQuestionSets_Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SurveyQuestionSets",
                newName: "SurveyName");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyQuestionSets_Name",
                table: "SurveyQuestionSets",
                newName: "IX_SurveyQuestionSets_SurveyName");
        }
    }
}
