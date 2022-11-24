using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class RemoveFK_SalesPoint_MinimumExecutionLimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MinimumExecutionLimits_SalesPoints_SalesPointId",
                table: "MinimumExecutionLimits");

            migrationBuilder.DropIndex(
                name: "IX_MinimumExecutionLimits_SalesPointId",
                table: "MinimumExecutionLimits");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MinimumExecutionLimits_SalesPointId",
                table: "MinimumExecutionLimits",
                column: "SalesPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_MinimumExecutionLimits_SalesPoints_SalesPointId",
                table: "MinimumExecutionLimits",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
