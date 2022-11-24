using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class RemoveFK_SalesPoint_Transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_SalesPoints_SalesPointId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SalesPointId",
                table: "Transactions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SalesPointId",
                table: "Transactions",
                column: "SalesPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_SalesPoints_SalesPointId",
                table: "Transactions",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
