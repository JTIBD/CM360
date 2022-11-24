using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class ModifyWorkFlowAndWorkFlowConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "WorkFlows",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NotificationStatus",
                table: "WorkFlowConfigurations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RejectedStatus",
                table: "WorkFlowConfigurations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlows_Code",
                table: "WorkFlows",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkFlows_Code",
                table: "WorkFlows");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "WorkFlows");

            migrationBuilder.DropColumn(
                name: "NotificationStatus",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropColumn(
                name: "RejectedStatus",
                table: "WorkFlowConfigurations");
        }
    }
}
