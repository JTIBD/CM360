using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class ProductAndPosmProductModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsJTIProduct",
                table: "Products",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsJTIProduct",
                table: "Products",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int));
         
        }
    }
}
