using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class RemoveFK_SalesPoint_DailyTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyTasks_SalesPoints_SalesPointId",
                table: "DailyTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyTasks_SalesPointId",
                table: "DailyTasks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DailyTasks_SalesPointId",
                table: "DailyTasks",
                column: "SalesPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTasks_SalesPoints_SalesPointId",
                table: "DailyTasks",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
