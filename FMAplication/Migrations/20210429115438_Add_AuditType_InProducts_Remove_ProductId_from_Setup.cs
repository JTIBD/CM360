using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Add_AuditType_InProducts_Remove_ProductId_from_Setup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "AuditSetups");

            migrationBuilder.AddColumn<int>(
                name: "AuditType",
                table: "AuditProducts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditType",
                table: "AuditProducts");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "AuditSetups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
