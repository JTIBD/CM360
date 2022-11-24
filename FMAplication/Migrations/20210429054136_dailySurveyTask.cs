using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class dailySurveyTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailySurveyTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DailyTaskId = table.Column<int>(nullable: false),
                    SurveyQuestionSetId = table.Column<int>(nullable: false),
                    TaskStatus = table.Column<int>(nullable: false),
                    Reason = table.Column<int>(nullable: false),
                    ReasonDetails = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailySurveyTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailySurveyTasks_DailyTasks_DailyTaskId",
                        column: x => x.DailyTaskId,
                        principalTable: "DailyTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailySurveyTasks_SurveyQuestionSets_SurveyQuestionSetId",
                        column: x => x.SurveyQuestionSetId,
                        principalTable: "SurveyQuestionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailySurveyTaskAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DailySurveyTaskId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailySurveyTaskAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailySurveyTaskAnswers_DailySurveyTasks_DailySurveyTaskId",
                        column: x => x.DailySurveyTaskId,
                        principalTable: "DailySurveyTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailySurveyTaskAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailySurveyTaskAnswers_DailySurveyTaskId",
                table: "DailySurveyTaskAnswers",
                column: "DailySurveyTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DailySurveyTaskAnswers_QuestionId",
                table: "DailySurveyTaskAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_DailySurveyTasks_DailyTaskId",
                table: "DailySurveyTasks",
                column: "DailyTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DailySurveyTasks_SurveyQuestionSetId",
                table: "DailySurveyTasks",
                column: "SurveyQuestionSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailySurveyTaskAnswers");

            migrationBuilder.DropTable(
                name: "DailySurveyTasks");
        }
    }
}
