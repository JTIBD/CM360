using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class DailyAvTask_Outlet_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DailyAvTasks_OutletId",
                table: "DailyAvTasks",
                column: "OutletId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyAvTasks_Outlets_OutletId",
                table: "DailyAvTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyAvTasks_Outlets_OutletId",
                table: "DailyAvTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyAvTasks_OutletId",
                table: "DailyAvTasks");
        }
    }
}
