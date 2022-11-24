using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class RemoveFK_SalesPoint_SP_Receives : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesPointReceivedTransfers_SalesPoints_ToSalesPointId",
                table: "SalesPointReceivedTransfers");

            migrationBuilder.DropIndex(
                name: "IX_SalesPointReceivedTransfers_ToSalesPointId",
                table: "SalesPointReceivedTransfers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SalesPointReceivedTransfers_ToSalesPointId",
                table: "SalesPointReceivedTransfers",
                column: "ToSalesPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPointReceivedTransfers_SalesPoints_ToSalesPointId",
                table: "SalesPointReceivedTransfers",
                column: "ToSalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
