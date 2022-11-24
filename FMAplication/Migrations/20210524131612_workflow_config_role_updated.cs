using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class workflow_config_role_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowConfigurations_OrganizationRoles_RoleId",
                table: "WorkFlowConfigurations");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFlowConfigurations_Roles_RoleId",
                table: "WorkFlowConfigurations",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowConfigurations_Roles_RoleId",
                table: "WorkFlowConfigurations");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFlowConfigurations_OrganizationRoles_RoleId",
                table: "WorkFlowConfigurations",
                column: "RoleId",
                principalTable: "OrganizationRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
