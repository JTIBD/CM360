using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class WDistributionTransactions_WDistributionRecieveTransactions_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WDistributionTransactions",
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
                    TransactionId = table.Column<int>(nullable: false),
                    POSMProductId = table.Column<int>(nullable: false),
                    SalesPointId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WDistributionTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WDistributionTransactions_POSMProducts_POSMProductId",
                        column: x => x.POSMProductId,
                        principalTable: "POSMProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WDistributionTransactions_SalesPoints_SalesPointId",
                        column: x => x.SalesPointId,
                        principalTable: "SalesPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WDistributionTransactions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WDistributionRecieveTransactions",
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
                    WDistributionTransactionId = table.Column<int>(nullable: false),
                    RecievedQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WDistributionRecieveTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WDistributionRecieveTransactions_WDistributionTransactions_WDistributionTransactionId",
                        column: x => x.WDistributionTransactionId,
                        principalTable: "WDistributionTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WDistributionRecieveTransactions_WDistributionTransactionId",
                table: "WDistributionRecieveTransactions",
                column: "WDistributionTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_WDistributionTransactions_POSMProductId",
                table: "WDistributionTransactions",
                column: "POSMProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WDistributionTransactions_SalesPointId",
                table: "WDistributionTransactions",
                column: "SalesPointId");

            migrationBuilder.CreateIndex(
                name: "IX_WDistributionTransactions_TransactionId",
                table: "WDistributionTransactions",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WDistributionRecieveTransactions");

            migrationBuilder.DropTable(
                name: "WDistributionTransactions");
        }
    }
}
