using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class DailyAuditTask_Outlet_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DailyAuditTasks_OutletId",
                table: "DailyAuditTasks",
                column: "OutletId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyAuditTasks_Outlets_OutletId",
                table: "DailyAuditTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyAuditTasks_Outlets_OutletId",
                table: "DailyAuditTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyAuditTasks_OutletId",
                table: "DailyAuditTasks");
        }
    }
}
