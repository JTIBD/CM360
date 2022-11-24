using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class RemoveFK_ProductId_DailyAuditProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyProductsAuditTasks_Products_ProductId",
                table: "DailyProductsAuditTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyProductsAuditTasks_ProductId",
                table: "DailyProductsAuditTasks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DailyProductsAuditTasks_ProductId",
                table: "DailyProductsAuditTasks",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyProductsAuditTasks_Products_ProductId",
                table: "DailyProductsAuditTasks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
