using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Remove_FK_SalesPointStock_SalesPointId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesPointStocks_SalesPoints_SalesPointId",
                table: "SalesPointStocks");

            migrationBuilder.DropIndex(
                name: "IX_SalesPointStocks_SalesPointId",
                table: "SalesPointStocks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SalesPointStocks_SalesPointId",
                table: "SalesPointStocks",
                column: "SalesPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPointStocks_SalesPoints_SalesPointId",
                table: "SalesPointStocks",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
