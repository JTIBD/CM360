using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class wfTablesUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WorkFlowTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TableName",
                table: "WorkflowLogs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowConfigurations_OrgRoleId",
                table: "WorkFlowConfigurations",
                column: "OrgRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFlowConfigurations_OrganizationRoles_OrgRoleId",
                table: "WorkFlowConfigurations",
                column: "OrgRoleId",
                principalTable: "OrganizationRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowConfigurations_OrganizationRoles_OrgRoleId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_WorkFlowConfigurations_OrgRoleId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WorkFlowTypes");

            migrationBuilder.DropColumn(
                name: "TableName",
                table: "WorkflowLogs");
        }
    }
}
