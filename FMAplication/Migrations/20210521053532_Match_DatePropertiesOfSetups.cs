using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Match_DatePropertiesOfSetups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToDate",
                table: "Surveys",
                newName: "ToDate");

            migrationBuilder.RenameColumn(
                name: "FromDate",
                table: "Surveys",
                newName: "FromDate");

            migrationBuilder.RenameColumn(
                name: "ToDate",
                table: "GuidelineSetups",
                newName: "ToDate");

            migrationBuilder.RenameColumn(
                name: "FromDate",
                table: "GuidelineSetups",
                newName: "FromDate");

            migrationBuilder.RenameColumn(
                name: "ToDate",
                table: "AvSetups",
                newName: "ToDate");

            migrationBuilder.RenameColumn(
                name: "FromDate",
                table: "AvSetups",
                newName: "FromDate");

            migrationBuilder.RenameColumn(
                name: "ToDate",
                table: "AuditSetups",
                newName: "ToDate");

            migrationBuilder.RenameColumn(
                name: "FromDate",
                table: "AuditSetups",
                newName: "FromDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToDate",
                table: "Surveys",
                newName: "ToDate");

            migrationBuilder.RenameColumn(
                name: "FromDate",
                table: "Surveys",
                newName: "FromDate");

            migrationBuilder.RenameColumn(
                name: "ToDate",
                table: "GuidelineSetups",
                newName: "ToDate");

            migrationBuilder.RenameColumn(
                name: "FromDate",
                table: "GuidelineSetups",
                newName: "FromDate");

            migrationBuilder.RenameColumn(
                name: "ToDate",
                table: "AvSetups",
                newName: "ToDate");

            migrationBuilder.RenameColumn(
                name: "FromDate",
                table: "AvSetups",
                newName: "FromDate");

            migrationBuilder.RenameColumn(
                name: "ToDate",
                table: "AuditSetups",
                newName: "ToDate");

            migrationBuilder.RenameColumn(
                name: "FromDate",
                table: "AuditSetups",
                newName: "FromDate");
        }
    }
}
