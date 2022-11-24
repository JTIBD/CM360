using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class workflowNotificationUpdatedAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkFlowTransactionId",
                table: "TransactionNotifications");

            migrationBuilder.AddColumn<int>(
                name: "SubmittedById",
                table: "TransactionWorkflows",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionWorkFlowId",
                table: "TransactionNotifications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionWorkflows_SubmittedById",
                table: "TransactionWorkflows",
                column: "SubmittedById");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionWorkflows_UserInfos_SubmittedById",
                table: "TransactionWorkflows",
                column: "SubmittedById",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionWorkflows_UserInfos_SubmittedById",
                table: "TransactionWorkflows");

            migrationBuilder.DropIndex(
                name: "IX_TransactionWorkflows_SubmittedById",
                table: "TransactionWorkflows");

            migrationBuilder.DropColumn(
                name: "SubmittedById",
                table: "TransactionWorkflows");

            migrationBuilder.DropColumn(
                name: "TransactionWorkFlowId",
                table: "TransactionNotifications");

            migrationBuilder.AddColumn<int>(
                name: "WorkFlowTransactionId",
                table: "TransactionNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
