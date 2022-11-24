using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class RemoveFK_SalesPoint_SP_transfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesPointTransfers_SalesPoints_FromSalesPointId",
                table: "SalesPointTransfers");

            migrationBuilder.DropIndex(
                name: "IX_SalesPointTransfers_FromSalesPointId",
                table: "SalesPointTransfers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SalesPointTransfers_FromSalesPointId",
                table: "SalesPointTransfers",
                column: "FromSalesPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPointTransfers_SalesPoints_FromSalesPointId",
                table: "SalesPointTransfers",
                column: "FromSalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
