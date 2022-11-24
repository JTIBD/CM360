using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class stockadjustUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentItems_POSMProducts_PosmProductId",
                table: "StockAdjustmentItems");

            migrationBuilder.DropColumn(
                name: "POSM_Id",
                table: "StockAdjustmentItems");

            migrationBuilder.AlterColumn<int>(
                name: "PosmProductId",
                table: "StockAdjustmentItems",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentItems_POSMProducts_PosmProductId",
                table: "StockAdjustmentItems",
                column: "PosmProductId",
                principalTable: "POSMProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentItems_POSMProducts_PosmProductId",
                table: "StockAdjustmentItems");

            migrationBuilder.AlterColumn<int>(
                name: "PosmProductId",
                table: "StockAdjustmentItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "POSM_Id",
                table: "StockAdjustmentItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentItems_POSMProducts_PosmProductId",
                table: "StockAdjustmentItems",
                column: "PosmProductId",
                principalTable: "POSMProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
