using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class auditResultTypeChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Result",
                table: "DailyProductsAuditTasks",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Result",
                table: "DailyProductsAuditTasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
