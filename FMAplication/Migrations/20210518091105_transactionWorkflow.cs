using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class transactionWorkflow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sequence",
                table: "WorkFlowConfigurations",
                newName: "Sequence");

            migrationBuilder.CreateTable(
                name: "TransactionWorkflow",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: false),
                    TWStatus = table.Column<int>(nullable: false),
                    TransactionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionWorkflow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionWorkflow_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionWorkflow_WorkFlows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "WorkFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionWorkflowLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TransactionWorkflowId = table.Column<int>(nullable: false),
                    TwStatus = table.Column<int>(nullable: false),
                    WorkFlowConfigurationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionWorkflowLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionWorkflowLogs_TransactionWorkflow_TransactionWorkflowId",
                        column: x => x.TransactionWorkflowId,
                        principalTable: "TransactionWorkflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionWorkflowLogs_WorkFlowConfigurations_WorkFlowConfigurationId",
                        column: x => x.WorkFlowConfigurationId,
                        principalTable: "WorkFlowConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionWorkflow_TransactionId",
                table: "TransactionWorkflow",
                column: "TransactionId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionWorkflowLogs");

            migrationBuilder.DropTable(
                name: "TransactionWorkflow");

            migrationBuilder.RenameColumn(
                name: "Sequence",
                table: "WorkFlowConfigurations",
                newName: "sequence");
        }
    }
}
