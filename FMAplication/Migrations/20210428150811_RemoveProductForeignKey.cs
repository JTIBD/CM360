using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class RemoveProductForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditSetups_Products_ProductId",
                table: "AuditSetups");

            migrationBuilder.DropIndex(
                name: "IX_AuditSetups_ProductId",
                table: "AuditSetups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AuditSetups_ProductId",
                table: "AuditSetups",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditSetups_Products_ProductId",
                table: "AuditSetups",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
