using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class transactionUpdatedAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_WareHouses_WareHouseId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CentralWarehouseId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "WareHouseId",
                table: "Transactions",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_WareHouseId",
                table: "Transactions",
                newName: "IX_Transactions_WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_WareHouses_WarehouseId",
                table: "Transactions",
                column: "WarehouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_WareHouses_WarehouseId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "Transactions",
                newName: "WareHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_WarehouseId",
                table: "Transactions",
                newName: "IX_Transactions_WareHouseId");

            migrationBuilder.AddColumn<int>(
                name: "CentralWarehouseId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_WareHouses_WareHouseId",
                table: "Transactions",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
