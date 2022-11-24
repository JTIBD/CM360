using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Table_WareHouseReceives_SalesPointTransfers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WareHouseTransactionItems_POSMProducts_POSMProductId",
                table: "WareHouseTransactionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WareHouseTransactionItems_WareHouseTransactions_TransactionId",
                table: "WareHouseTransactionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WareHouseTransactions_WareHouses_FromWarehouseId",
                table: "WareHouseTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WareHouseTransactions",
                table: "WareHouseTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WareHouseTransactionItems",
                table: "WareHouseTransactionItems");

            migrationBuilder.RenameTable(
                name: "WareHouseTransactions",
                newName: "WareHouseTransfer");

            migrationBuilder.RenameTable(
                name: "WareHouseTransactionItems",
                newName: "WareHouseTransferItem");

            migrationBuilder.RenameIndex(
                name: "IX_WareHouseTransactions_FromWarehouseId",
                table: "WareHouseTransfer",
                newName: "IX_WareHouseTransfer_FromWarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_WareHouseTransactionItems_TransactionId",
                table: "WareHouseTransferItem",
                newName: "IX_WareHouseTransferItem_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_WareHouseTransactionItems_POSMProductId",
                table: "WareHouseTransferItem",
                newName: "IX_WareHouseTransferItem_POSMProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WareHouseTransfer",
                table: "WareHouseTransfer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WareHouseTransferItem",
                table: "WareHouseTransferItem",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SalesPointReceivedTransfers",
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
                    FromSalesPointId = table.Column<int>(nullable: false),
                    ToSalesPointId = table.Column<int>(nullable: false),
                    SourceTransferId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPointReceivedTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesPointReceivedTransfers_SalesPoints_ToSalesPointId",
                        column: x => x.ToSalesPointId,
                        principalTable: "SalesPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WareHouseReceivedTransfers",
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
                    ToWarehouseId = table.Column<int>(nullable: false),
                    SourceTransferId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseReceivedTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WareHouseReceivedTransfers_WareHouses_ToWarehouseId",
                        column: x => x.ToWarehouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesPointReceivedTransferItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TransferId = table.Column<int>(nullable: false),
                    POSMProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ReceivedQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPointReceivedTransferItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesPointReceivedTransferItems_POSMProducts_POSMProductId",
                        column: x => x.POSMProductId,
                        principalTable: "POSMProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesPointReceivedTransferItems_SalesPointReceivedTransfers_TransferId",
                        column: x => x.TransferId,
                        principalTable: "SalesPointReceivedTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WareHouseReceivedTransferItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TransferId = table.Column<int>(nullable: false),
                    POSMProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ReceivedQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseReceivedTransferItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WareHouseReceivedTransferItems_POSMProducts_POSMProductId",
                        column: x => x.POSMProductId,
                        principalTable: "POSMProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WareHouseReceivedTransferItems_WareHouseReceivedTransfers_TransferId",
                        column: x => x.TransferId,
                        principalTable: "WareHouseReceivedTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesPointReceivedTransferItems_POSMProductId",
                table: "SalesPointReceivedTransferItems",
                column: "POSMProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPointReceivedTransferItems_TransferId",
                table: "SalesPointReceivedTransferItems",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPointReceivedTransfers_ToSalesPointId",
                table: "SalesPointReceivedTransfers",
                column: "ToSalesPointId");

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseReceivedTransferItems_POSMProductId",
                table: "WareHouseReceivedTransferItems",
                column: "POSMProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseReceivedTransferItems_TransferId",
                table: "WareHouseReceivedTransferItems",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseReceivedTransfers_ToWarehouseId",
                table: "WareHouseReceivedTransfers",
                column: "ToWarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_WareHouseTransfer_WareHouses_FromWarehouseId",
                table: "WareHouseTransfer",
                column: "FromWarehouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WareHouseTransferItem_POSMProducts_POSMProductId",
                table: "WareHouseTransferItem",
                column: "POSMProductId",
                principalTable: "POSMProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WareHouseTransferItem_WareHouseTransfer_TransactionId",
                table: "WareHouseTransferItem",
                column: "TransactionId",
                principalTable: "WareHouseTransfer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WareHouseTransfer_WareHouses_FromWarehouseId",
                table: "WareHouseTransfer");

            migrationBuilder.DropForeignKey(
                name: "FK_WareHouseTransferItem_POSMProducts_POSMProductId",
                table: "WareHouseTransferItem");

            migrationBuilder.DropForeignKey(
                name: "FK_WareHouseTransferItem_WareHouseTransfer_TransactionId",
                table: "WareHouseTransferItem");

            migrationBuilder.DropTable(
                name: "SalesPointReceivedTransferItems");

            migrationBuilder.DropTable(
                name: "WareHouseReceivedTransferItems");

            migrationBuilder.DropTable(
                name: "SalesPointReceivedTransfers");

            migrationBuilder.DropTable(
                name: "WareHouseReceivedTransfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WareHouseTransferItem",
                table: "WareHouseTransferItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WareHouseTransfer",
                table: "WareHouseTransfer");

            migrationBuilder.RenameTable(
                name: "WareHouseTransferItem",
                newName: "WareHouseTransactionItems");

            migrationBuilder.RenameTable(
                name: "WareHouseTransfer",
                newName: "WareHouseTransactions");

            migrationBuilder.RenameIndex(
                name: "IX_WareHouseTransferItem_TransactionId",
                table: "WareHouseTransactionItems",
                newName: "IX_WareHouseTransactionItems_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_WareHouseTransferItem_POSMProductId",
                table: "WareHouseTransactionItems",
                newName: "IX_WareHouseTransactionItems_POSMProductId");

            migrationBuilder.RenameIndex(
                name: "IX_WareHouseTransfer_FromWarehouseId",
                table: "WareHouseTransactions",
                newName: "IX_WareHouseTransactions_FromWarehouseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WareHouseTransactionItems",
                table: "WareHouseTransactionItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WareHouseTransactions",
                table: "WareHouseTransactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WareHouseTransactionItems_POSMProducts_POSMProductId",
                table: "WareHouseTransactionItems",
                column: "POSMProductId",
                principalTable: "POSMProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WareHouseTransactionItems_WareHouseTransactions_TransactionId",
                table: "WareHouseTransactionItems",
                column: "TransactionId",
                principalTable: "WareHouseTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WareHouseTransactions_WareHouses_FromWarehouseId",
                table: "WareHouseTransactions",
                column: "FromWarehouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
