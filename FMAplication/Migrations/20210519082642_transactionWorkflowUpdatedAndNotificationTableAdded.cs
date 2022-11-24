using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class transactionWorkflowUpdatedAndNotificationTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionWorkflow_Transactions_TransactionId",
                table: "TransactionWorkflow");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionWorkflow_WorkFlows_WorkflowId",
                table: "TransactionWorkflow");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowConfigurations_WorkFlows_MasterWorkFlowId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropTable(
                name: "TransactionWorkflowLogs");

            migrationBuilder.DropIndex(
                name: "IX_WorkFlowConfigurations_MasterWorkFlowId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionWorkflow",
                table: "TransactionWorkflow");

            migrationBuilder.DropIndex(
                name: "IX_TransactionWorkflow_WorkflowId",
                table: "TransactionWorkflow");

            migrationBuilder.DropColumn(
                name: "MasterWorkFlowId",
                table: "WorkFlowConfigurations");

            migrationBuilder.RenameTable(
                name: "TransactionWorkflow",
                newName: "TransactionWorkflows");

            migrationBuilder.RenameColumn(
                name: "WorkflowId",
                table: "TransactionWorkflows",
                newName: "WorkFlowId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionWorkflow_TransactionId",
                table: "TransactionWorkflows",
                newName: "IX_TransactionWorkflows_TransactionId");

            migrationBuilder.AddColumn<int>(
                name: "WorkFlowId",
                table: "WorkFlowConfigurations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "TransactionWorkflows",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "TransactionWorkflows",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TransactionWorkflows",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowConfigurationId",
                table: "TransactionWorkflows",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionWorkflows",
                table: "TransactionWorkflows",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TransactionNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    TransactionId = table.Column<int>(nullable: false),
                    IsSeen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionNotifications_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionNotifications_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowConfigurations_WorkFlowId",
                table: "WorkFlowConfigurations",
                column: "WorkFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionWorkflows_RoleId",
                table: "TransactionWorkflows",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionWorkflows_UserId",
                table: "TransactionWorkflows",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionNotifications_TransactionId",
                table: "TransactionNotifications",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionNotifications_UserId",
                table: "TransactionNotifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionWorkflows_OrganizationRoles_RoleId",
                table: "TransactionWorkflows",
                column: "RoleId",
                principalTable: "OrganizationRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionWorkflows_Transactions_TransactionId",
                table: "TransactionWorkflows",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionWorkflows_UserInfos_UserId",
                table: "TransactionWorkflows",
                column: "UserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFlowConfigurations_WorkFlows_WorkFlowId",
                table: "WorkFlowConfigurations",
                column: "WorkFlowId",
                principalTable: "WorkFlows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionWorkflows_OrganizationRoles_RoleId",
                table: "TransactionWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionWorkflows_Transactions_TransactionId",
                table: "TransactionWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionWorkflows_UserInfos_UserId",
                table: "TransactionWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowConfigurations_WorkFlows_WorkFlowId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropTable(
                name: "TransactionNotifications");

            migrationBuilder.DropIndex(
                name: "IX_WorkFlowConfigurations_WorkFlowId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionWorkflows",
                table: "TransactionWorkflows");

            migrationBuilder.DropIndex(
                name: "IX_TransactionWorkflows_RoleId",
                table: "TransactionWorkflows");

            migrationBuilder.DropIndex(
                name: "IX_TransactionWorkflows_UserId",
                table: "TransactionWorkflows");

            migrationBuilder.DropColumn(
                name: "WorkFlowId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "TransactionWorkflows");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "TransactionWorkflows");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TransactionWorkflows");

            migrationBuilder.DropColumn(
                name: "WorkflowConfigurationId",
                table: "TransactionWorkflows");

            migrationBuilder.RenameTable(
                name: "TransactionWorkflows",
                newName: "TransactionWorkflow");

            migrationBuilder.RenameColumn(
                name: "WorkFlowId",
                table: "TransactionWorkflow",
                newName: "WorkflowId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionWorkflows_TransactionId",
                table: "TransactionWorkflow",
                newName: "IX_TransactionWorkflow_TransactionId");

            migrationBuilder.AddColumn<int>(
                name: "MasterWorkFlowId",
                table: "WorkFlowConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionWorkflow",
                table: "TransactionWorkflow",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TransactionWorkflowLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TWLogStatus = table.Column<int>(type: "int", nullable: false),
                    TransactionWorkflowId = table.Column<int>(type: "int", nullable: false),
                    WorkFlowConfigurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionWorkflowLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionWorkflowLogs_TransactionWorkflow_TransactionWorkflowId",
                        column: x => x.TransactionWorkflowId,
                        principalTable: "TransactionWorkflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionWorkflowLogs_WorkFlowConfigurations_WorkFlowConfigurationId",
                        column: x => x.WorkFlowConfigurationId,
                        principalTable: "WorkFlowConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowConfigurations_MasterWorkFlowId",
                table: "WorkFlowConfigurations",
                column: "MasterWorkFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionWorkflow_WorkflowId",
                table: "TransactionWorkflow",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionWorkflowLogs_TransactionWorkflowId",
                table: "TransactionWorkflowLogs",
                column: "TransactionWorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionWorkflowLogs_WorkFlowConfigurationId",
                table: "TransactionWorkflowLogs",
                column: "WorkFlowConfigurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionWorkflow_Transactions_TransactionId",
                table: "TransactionWorkflow",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionWorkflow_WorkFlows_WorkflowId",
                table: "TransactionWorkflow",
                column: "WorkflowId",
                principalTable: "WorkFlows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFlowConfigurations_WorkFlows_MasterWorkFlowId",
                table: "WorkFlowConfigurations",
                column: "MasterWorkFlowId",
                principalTable: "WorkFlows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
