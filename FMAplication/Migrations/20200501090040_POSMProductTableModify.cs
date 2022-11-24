using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class POSMProductTableModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPlanogram",
                table: "POSMProducts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PlanogramImageUrl",
                table: "POSMProducts",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPlanogram",
                table: "POSMProducts");

            migrationBuilder.DropColumn(
                name: "PlanogramImageUrl",
                table: "POSMProducts");
        }
    }
}
