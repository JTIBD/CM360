using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class workflowConfigurationChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowConfigurations_OrganizationRoles_OrgRoleId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_WorkFlowConfigurations_OrgRoleId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropColumn(
                name: "ModeOfApproval",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropColumn(
                name: "NotificationStatus",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropColumn(
                name: "OrgRoleId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropColumn(
                name: "ReceivedStatus",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropColumn(
                name: "RejectedStatus",
                table: "WorkFlowConfigurations");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "WorkFlowConfigurations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "WorkFlowConfigurations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowConfigurations_RoleId",
                table: "WorkFlowConfigurations",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowConfigurations_UserId",
                table: "WorkFlowConfigurations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFlowConfigurations_OrganizationRoles_RoleId",
                table: "WorkFlowConfigurations",
                column: "RoleId",
                principalTable: "OrganizationRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFlowConfigurations_UserInfos_UserId",
                table: "WorkFlowConfigurations",
                column: "UserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowConfigurations_OrganizationRoles_RoleId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowConfigurations_UserInfos_UserId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_WorkFlowConfigurations_RoleId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_WorkFlowConfigurations_UserId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkFlowConfigurations");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "WorkFlowConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModeOfApproval",
                table: "WorkFlowConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NotificationStatus",
                table: "WorkFlowConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrgRoleId",
                table: "WorkFlowConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReceivedStatus",
                table: "WorkFlowConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectedStatus",
                table: "WorkFlowConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
