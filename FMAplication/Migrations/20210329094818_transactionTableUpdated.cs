using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class transactionTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SyoAdjustmentItems_POSMProducts_PosmProductId",
                table: "SyoAdjustmentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SyoAdjustmentItems_Transactions_TransactionId",
                table: "SyoAdjustmentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SyoAdjustmentItems_WareHouses_WareHouseId",
                table: "SyoAdjustmentItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SyoAdjustmentItems",
                table: "SyoAdjustmentItems");

            migrationBuilder.DropColumn(
                name: "ActualQuantity",
                table: "SyoAdjustmentItems");

            migrationBuilder.RenameTable(
                name: "SyoAdjustmentItems",
                newName: "StockAdjustmentItems");

            migrationBuilder.RenameIndex(
                name: "IX_SyoAdjustmentItems_WareHouseId",
                table: "StockAdjustmentItems",
                newName: "IX_StockAdjustmentItems_WareHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_SyoAdjustmentItems_TransactionId",
                table: "StockAdjustmentItems",
                newName: "IX_StockAdjustmentItems_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_SyoAdjustmentItems_PosmProductId",
                table: "StockAdjustmentItems",
                newName: "IX_StockAdjustmentItems_PosmProductId");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionType",
                table: "Transactions",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionStatus",
                table: "Transactions",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SystemQuantity",
                table: "StockAdjustmentItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockAdjustmentItems",
                table: "StockAdjustmentItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentItems_POSMProducts_PosmProductId",
                table: "StockAdjustmentItems",
                column: "PosmProductId",
                principalTable: "POSMProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentItems_Transactions_TransactionId",
                table: "StockAdjustmentItems",
                column: "TransactionId",
                principalTable: "Transactions",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentItems_POSMProducts_PosmProductId",
                table: "StockAdjustmentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentItems_Transactions_TransactionId",
                table: "StockAdjustmentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentItems_WareHouses_WareHouseId",
                table: "StockAdjustmentItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockAdjustmentItems",
                table: "StockAdjustmentItems");

            migrationBuilder.DropColumn(
                name: "SystemQuantity",
                table: "StockAdjustmentItems");

            migrationBuilder.RenameTable(
                name: "StockAdjustmentItems",
                newName: "SyoAdjustmentItems");

            migrationBuilder.RenameIndex(
                name: "IX_StockAdjustmentItems_WareHouseId",
                table: "SyoAdjustmentItems",
                newName: "IX_SyoAdjustmentItems_WareHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_StockAdjustmentItems_TransactionId",
                table: "SyoAdjustmentItems",
                newName: "IX_SyoAdjustmentItems_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_StockAdjustmentItems_PosmProductId",
                table: "SyoAdjustmentItems",
                newName: "IX_SyoAdjustmentItems_PosmProductId");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TransactionStatus",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActualQuantity",
                table: "SyoAdjustmentItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SyoAdjustmentItems",
                table: "SyoAdjustmentItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SyoAdjustmentItems_POSMProducts_PosmProductId",
                table: "SyoAdjustmentItems",
                column: "PosmProductId",
                principalTable: "POSMProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SyoAdjustmentItems_Transactions_TransactionId",
                table: "SyoAdjustmentItems",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SyoAdjustmentItems_WareHouses_WareHouseId",
                table: "SyoAdjustmentItems",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
