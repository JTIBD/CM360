using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class WorkFlowLogHistoryTableModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSeen",
                table: "WorkflowLogHistories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WorkflowTitle",
                table: "WorkflowLogHistories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSeen",
                table: "WorkflowLogHistories");

            migrationBuilder.DropColumn(
                name: "WorkflowTitle",
                table: "WorkflowLogHistories");
        }
    }
}
