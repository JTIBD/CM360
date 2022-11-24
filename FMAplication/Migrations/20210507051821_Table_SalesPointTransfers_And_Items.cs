using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Table_SalesPointTransfers_And_Items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesPointTransfers",
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
                    ToSalesPointId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPointTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesPointTransfers_SalesPoints_FromSalesPointId",
                        column: x => x.FromSalesPointId,
                        principalTable: "SalesPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesPointTransferItems",
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
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPointTransferItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesPointTransferItems_POSMProducts_POSMProductId",
                        column: x => x.POSMProductId,
                        principalTable: "POSMProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesPointTransferItems_SalesPointTransfers_TransferId",
                        column: x => x.TransferId,
                        principalTable: "SalesPointTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesPointTransferItems_POSMProductId",
                table: "SalesPointTransferItems",
                column: "POSMProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPointTransferItems_TransferId",
                table: "SalesPointTransferItems",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPointTransfers_FromSalesPointId",
                table: "SalesPointTransfers",
                column: "FromSalesPointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesPointTransferItems");

            migrationBuilder.DropTable(
                name: "SalesPointTransfers");
        }
    }
}
