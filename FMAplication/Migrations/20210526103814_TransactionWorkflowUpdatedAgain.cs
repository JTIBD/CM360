using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class TransactionWorkflowUpdatedAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionWorkflows_OrganizationRoles_RoleId",
                table: "TransactionWorkflows");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionWorkflows_Roles_RoleId",
                table: "TransactionWorkflows",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionWorkflows_Roles_RoleId",
                table: "TransactionWorkflows");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionWorkflows_OrganizationRoles_RoleId",
                table: "TransactionWorkflows",
                column: "RoleId",
                principalTable: "OrganizationRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
