using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class CodeAddInBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "SubBrands",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Brands",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "SubBrands");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Brands");
        }
    }
}
