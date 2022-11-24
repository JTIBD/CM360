using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Update_WDistributionRecieveTransaction_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WDistributionRecieveTransactions_WDistributionTransactions_WDistributionTransactionId",
                table: "WDistributionRecieveTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WDistributionRecieveTransactions_WDistributionTransactionId",
                table: "WDistributionRecieveTransactions");

            migrationBuilder.DropColumn(
                name: "WDistributionTransactionId",
                table: "WDistributionRecieveTransactions");

            migrationBuilder.AddColumn<int>(
                name: "POSMProductId",
                table: "WDistributionRecieveTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "WDistributionRecieveTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WDistributionRecieveTransactions_POSMProductId",
                table: "WDistributionRecieveTransactions",
                column: "POSMProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WDistributionRecieveTransactions_TransactionId",
                table: "WDistributionRecieveTransactions",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_WDistributionRecieveTransactions_POSMProducts_POSMProductId",
                table: "WDistributionRecieveTransactions",
                column: "POSMProductId",
                principalTable: "POSMProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WDistributionRecieveTransactions_Transactions_TransactionId",
                table: "WDistributionRecieveTransactions",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WDistributionRecieveTransactions_POSMProducts_POSMProductId",
                table: "WDistributionRecieveTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WDistributionRecieveTransactions_Transactions_TransactionId",
                table: "WDistributionRecieveTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WDistributionRecieveTransactions_POSMProductId",
                table: "WDistributionRecieveTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WDistributionRecieveTransactions_TransactionId",
                table: "WDistributionRecieveTransactions");

            migrationBuilder.DropColumn(
                name: "POSMProductId",
                table: "WDistributionRecieveTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "WDistributionRecieveTransactions");

            migrationBuilder.AddColumn<int>(
                name: "WDistributionTransactionId",
                table: "WDistributionRecieveTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WDistributionRecieveTransactions_WDistributionTransactionId",
                table: "WDistributionRecieveTransactions",
                column: "WDistributionTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_WDistributionRecieveTransactions_WDistributionTransactions_WDistributionTransactionId",
                table: "WDistributionRecieveTransactions",
                column: "WDistributionTransactionId",
                principalTable: "WDistributionTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
