using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class DailyInformationTask_Outlet_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DailyInformationTasks_OutletId",
                table: "DailyInformationTasks",
                column: "OutletId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyInformationTasks_Outlets_OutletId",
                table: "DailyInformationTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyInformationTasks_Outlets_OutletId",
                table: "DailyInformationTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyInformationTasks_OutletId",
                table: "DailyInformationTasks");
        }
    }
}
