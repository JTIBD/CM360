using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Remote_WDistributionTransaction_SalespointId_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WDistributionTransactions_SalesPoints_SalesPointId",
                table: "WDistributionTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WDistributionTransactions_SalesPointId",
                table: "WDistributionTransactions");

            migrationBuilder.DropColumn(
                name: "SalesPointId",
                table: "WDistributionTransactions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalesPointId",
                table: "WDistributionTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WDistributionTransactions_SalesPointId",
                table: "WDistributionTransactions",
                column: "SalesPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_WDistributionTransactions_SalesPoints_SalesPointId",
                table: "WDistributionTransactions",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
