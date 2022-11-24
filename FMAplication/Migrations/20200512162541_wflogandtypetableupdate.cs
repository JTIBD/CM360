using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class wflogandtypetableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkflowMessage",
                table: "WorkFlowTypes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrgRoleId",
                table: "WorkflowLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkflowMessage",
                table: "WorkFlowTypes");

            migrationBuilder.DropColumn(
                name: "OrgRoleId",
                table: "WorkflowLogs");
        }
    }
}
