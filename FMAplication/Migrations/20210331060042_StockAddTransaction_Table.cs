using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class StockAddTransaction_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockAddTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    WareHouseId = table.Column<int>(nullable: false),
                    TransactionId = table.Column<int>(nullable: false),
                    PosmProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAddTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockAddTransactions_POSMProducts_PosmProductId",
                        column: x => x.PosmProductId,
                        principalTable: "POSMProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockAddTransactions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockAddTransactions_WareHouses_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockAddTransactions_PosmProductId",
                table: "StockAddTransactions",
                column: "PosmProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAddTransactions_TransactionId",
                table: "StockAddTransactions",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAddTransactions_WareHouseId",
                table: "StockAddTransactions",
                column: "WareHouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockAddTransactions");
        }
    }
}
