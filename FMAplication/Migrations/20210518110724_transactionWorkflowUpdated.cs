using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class transactionWorkflowUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwStatus",
                table: "TransactionWorkflowLogs");

            migrationBuilder.AddColumn<int>(
                name: "TWLogStatus",
                table: "TransactionWorkflowLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TWLogStatus",
                table: "TransactionWorkflowLogs");

            migrationBuilder.AddColumn<int>(
                name: "TwStatus",
                table: "TransactionWorkflowLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
