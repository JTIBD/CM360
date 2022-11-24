using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class workflowChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "WorkFlows");

            migrationBuilder.AddColumn<int>(
                name: "WorkflowConfigType",
                table: "WorkFlows",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkflowConfigType",
                table: "WorkFlows");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "WorkFlows",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }
    }
}
