using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class transactionUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAddTransactions_WareHouses_WareHouseId",
                table: "StockAddTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentItems_WareHouses_WareHouseId",
                table: "StockAdjustmentItems");

            migrationBuilder.DropIndex(
                name: "IX_StockAdjustmentItems_WareHouseId",
                table: "StockAdjustmentItems");

            migrationBuilder.DropIndex(
                name: "IX_StockAddTransactions_WareHouseId",
                table: "StockAddTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionSerial",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "StockAdjustmentItems");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "StockAddTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TransactionStatus",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CentralWarehouseId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChalanNumber",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesPointId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionNumber",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SalesPointId",
                table: "Transactions",
                column: "SalesPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_WareHouseId",
                table: "Transactions",
                column: "WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_SalesPoints_SalesPointId",
                table: "Transactions",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_WareHouses_WareHouseId",
                table: "Transactions",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_SalesPoints_SalesPointId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_WareHouses_WareHouseId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SalesPointId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_WareHouseId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CentralWarehouseId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ChalanNumber",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SalesPointId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionNumber",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionType",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "TransactionStatus",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TransactionSerial",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "StockAdjustmentItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "StockAddTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentItems_WareHouseId",
                table: "StockAdjustmentItems",
                column: "WareHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAddTransactions_WareHouseId",
                table: "StockAddTransactions",
                column: "WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAddTransactions_WareHouses_WareHouseId",
                table: "StockAddTransactions",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentItems_WareHouses_WareHouseId",
                table: "StockAdjustmentItems",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
