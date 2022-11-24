using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Table_Rename_WareHouseTransfer_And_WareHouseTransferItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WareHouseTransferItem");

            migrationBuilder.DropTable(
                name: "WareHouseTransfer");

            migrationBuilder.CreateTable(
                name: "WareHouseTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TransactionNumber = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    IsConfirmed = table.Column<bool>(nullable: false),
                    TransactionStatus = table.Column<int>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    FromWarehouseId = table.Column<int>(nullable: false),
                    ToWarehouseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WareHouseTransfers_WareHouses_FromWarehouseId",
                        column: x => x.FromWarehouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WareHouseTransferItems",
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
                    table.PrimaryKey("PK_WareHouseTransferItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WareHouseTransferItems_POSMProducts_POSMProductId",
                        column: x => x.POSMProductId,
                        principalTable: "POSMProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WareHouseTransferItems_WareHouseTransfers_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "WareHouseTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseTransferItems_POSMProductId",
                table: "WareHouseTransferItems",
                column: "POSMProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseTransferItems_TransactionId",
                table: "WareHouseTransferItems",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseTransfers_FromWarehouseId",
                table: "WareHouseTransfers",
                column: "FromWarehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WareHouseTransferItems");

            migrationBuilder.DropTable(
                name: "WareHouseTransfers");

            migrationBuilder.CreateTable(
                name: "WareHouseTransfer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromWarehouseId = table.Column<int>(type: "int", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ToWarehouseId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WareHouseTransfer_WareHouses_FromWarehouseId",
                        column: x => x.FromWarehouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WareHouseTransferItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    POSMProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseTransferItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WareHouseTransferItem_POSMProducts_POSMProductId",
                        column: x => x.POSMProductId,
                        principalTable: "POSMProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WareHouseTransferItem_WareHouseTransfer_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "WareHouseTransfer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseTransfer_FromWarehouseId",
                table: "WareHouseTransfer",
                column: "FromWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseTransferItem_POSMProductId",
                table: "WareHouseTransferItem",
                column: "POSMProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseTransferItem_TransactionId",
                table: "WareHouseTransferItem",
                column: "TransactionId");
        }
    }
}
