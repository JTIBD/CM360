using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class dailyConsumerSurveyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyConsumerSurveyTaskAnswers",
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
                    table.PrimaryKey("PK_DailyConsumerSurveyTaskAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyConsumerSurveyTaskAnswers_DailySurveyTasks_DailySurveyTaskId",
                        column: x => x.DailySurveyTaskId,
                        principalTable: "DailySurveyTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyConsumerSurveyTaskAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyConsumerSurveyTasks",
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
                    IsCompleted = table.Column<bool>(nullable: false),
                    Reason = table.Column<int>(nullable: true),
                    ReasonDetails = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyConsumerSurveyTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyConsumerSurveyTasks_DailyTasks_DailyTaskId",
                        column: x => x.DailyTaskId,
                        principalTable: "DailyTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyConsumerSurveyTasks_SurveyQuestionSets_SurveyQuestionSetId",
                        column: x => x.SurveyQuestionSetId,
                        principalTable: "SurveyQuestionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyConsumerSurveyTaskAnswers_DailySurveyTaskId",
                table: "DailyConsumerSurveyTaskAnswers",
                column: "DailySurveyTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyConsumerSurveyTaskAnswers_QuestionId",
                table: "DailyConsumerSurveyTaskAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyConsumerSurveyTasks_DailyTaskId",
                table: "DailyConsumerSurveyTasks",
                column: "DailyTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyConsumerSurveyTasks_SurveyQuestionSetId",
                table: "DailyConsumerSurveyTasks",
                column: "SurveyQuestionSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyConsumerSurveyTaskAnswers");

            migrationBuilder.DropTable(
                name: "DailyConsumerSurveyTasks");
        }
    }
}
