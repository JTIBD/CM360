using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Add_WareHouseTransaction__FromWarehouse_foreignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WareHouseTransactions_WareHouses_ToWarehouseId",
                table: "WareHouseTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WareHouseTransactions_ToWarehouseId",
                table: "WareHouseTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseTransactions_FromWarehouseId",
                table: "WareHouseTransactions",
                column: "FromWarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_WareHouseTransactions_WareHouses_FromWarehouseId",
                table: "WareHouseTransactions",
                column: "FromWarehouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WareHouseTransactions_WareHouses_FromWarehouseId",
                table: "WareHouseTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WareHouseTransactions_FromWarehouseId",
                table: "WareHouseTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseTransactions_ToWarehouseId",
                table: "WareHouseTransactions",
                column: "ToWarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_WareHouseTransactions_WareHouses_ToWarehouseId",
                table: "WareHouseTransactions",
                column: "ToWarehouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
