using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Survey_SurveyQuestionSetId_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SurveyQuestionSetId",
                table: "Surveys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_SurveyQuestionSetId",
                table: "Surveys",
                column: "SurveyQuestionSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_SurveyQuestionSets_SurveyQuestionSetId",
                table: "Surveys",
                column: "SurveyQuestionSetId",
                principalTable: "SurveyQuestionSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_SurveyQuestionSets_SurveyQuestionSetId",
                table: "Surveys");

            migrationBuilder.DropIndex(
                name: "IX_Surveys_SurveyQuestionSetId",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "SurveyQuestionSetId",
                table: "Surveys");
        }
    }
}
