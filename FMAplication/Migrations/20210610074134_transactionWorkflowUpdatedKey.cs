using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class transactionWorkflowUpdatedKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionNotifications_Transactions_TransactionId",
                table: "TransactionNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionWorkflows_Transactions_TransactionId",
                table: "TransactionWorkflows");

            migrationBuilder.DropIndex(
                name: "IX_TransactionWorkflows_TransactionId",
                table: "TransactionWorkflows");

            migrationBuilder.DropIndex(
                name: "IX_TransactionNotifications_TransactionId",
                table: "TransactionNotifications");

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "TransactionWorkflows",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "TransactionNotifications",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "TransactionWorkflows");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "TransactionNotifications");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionWorkflows_TransactionId",
                table: "TransactionWorkflows",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionNotifications_TransactionId",
                table: "TransactionNotifications",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionNotifications_Transactions_TransactionId",
                table: "TransactionNotifications",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionWorkflows_Transactions_TransactionId",
                table: "TransactionWorkflows",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
