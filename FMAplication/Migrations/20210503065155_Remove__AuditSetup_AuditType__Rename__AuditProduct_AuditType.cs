using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Remove__AuditSetup_AuditType__Rename__AuditProduct_AuditType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditType",
                table: "AuditSetups");

            migrationBuilder.RenameColumn(
                name: "AuditType",
                table: "AuditProducts",
                newName: "ActionType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActionType",
                table: "AuditProducts",
                newName: "AuditType");

            migrationBuilder.AddColumn<int>(
                name: "AuditType",
                table: "AuditSetups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
