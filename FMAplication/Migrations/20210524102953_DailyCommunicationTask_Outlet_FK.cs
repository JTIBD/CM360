using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class DailyCommunicationTask_Outlet_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DailyCommunicationTasks_OutletId",
                table: "DailyCommunicationTasks",
                column: "OutletId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyCommunicationTasks_Outlets_OutletId",
                table: "DailyCommunicationTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyCommunicationTasks_Outlets_OutletId",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyCommunicationTasks_OutletId",
                table: "DailyCommunicationTasks");
        }
    }
}
