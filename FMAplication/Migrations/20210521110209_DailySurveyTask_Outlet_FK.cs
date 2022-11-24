using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class DailySurveyTask_Outlet_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DailySurveyTasks_OutletId",
                table: "DailySurveyTasks",
                column: "OutletId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailySurveyTasks_Outlets_OutletId",
                table: "DailySurveyTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailySurveyTasks_Outlets_OutletId",
                table: "DailySurveyTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailySurveyTasks_OutletId",
                table: "DailySurveyTasks");
        }
    }
}
