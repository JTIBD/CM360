using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Cleaning_afterUnify_Tasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OutletId",
                table: "DailyConsumerSurveyTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DailyConsumerSurveyTasks_OutletId",
                table: "DailyConsumerSurveyTasks",
                column: "OutletId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyConsumerSurveyTasks_Outlets_OutletId",
                table: "DailyConsumerSurveyTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyConsumerSurveyTasks_Outlets_OutletId",
                table: "DailyConsumerSurveyTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyConsumerSurveyTasks_OutletId",
                table: "DailyConsumerSurveyTasks");

            migrationBuilder.DropColumn(
                name: "OutletId",
                table: "DailyConsumerSurveyTasks");
        }
    }
}
