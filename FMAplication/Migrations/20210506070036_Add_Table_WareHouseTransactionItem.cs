using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Add_Table_WareHouseTransactionItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WareHouseTransactionItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TransactionId = table.Column<int>(nullable: false),
                    POSMProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseTransactionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WareHouseTransactionItems_POSMProducts_POSMProductId",
                        column: x => x.POSMProductId,
                        principalTable: "POSMProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WareHouseTransactionItems_WareHouseTransactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "WareHouseTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseTransactionItems_POSMProductId",
                table: "WareHouseTransactionItems",
                column: "POSMProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseTransactionItems_TransactionId",
                table: "WareHouseTransactionItems",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WareHouseTransactionItems");
        }
    }
}
