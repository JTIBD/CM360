using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class workflowTypetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        

            migrationBuilder.CreateTable(
                name: "WorkFlowTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    WorkflowTypeId = table.Column<int>(nullable: false),
                    WorkflowTypeName = table.Column<string>(maxLength: 256, nullable: true),
                    DbTableName = table.Column<string>(maxLength: 256, nullable: true),
                    IsWorkflowDefAvailable = table.Column<bool>(nullable: false),
                    IsWorkflowConfigAvailable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowTypes", x => x.Id);
                });

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {           

            migrationBuilder.DropTable(
                name: "WorkFlowTypes");
         }
    }
}
