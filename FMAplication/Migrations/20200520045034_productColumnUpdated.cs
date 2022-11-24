using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class productColumnUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                nullable: true);            

            migrationBuilder.CreateIndex(
                name: "IX_POSMReports_ProductId",
                table: "POSMReports",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyCMActivities_OutletId",
                table: "DailyCMActivities",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditReports_ProductId",
                table: "AuditReports",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditReports_Products_ProductId",
                table: "AuditReports",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyCMActivities_Outlets_OutletId",
                table: "DailyCMActivities",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_POSMReports_POSMProducts_ProductId",
                table: "POSMReports",
                column: "ProductId",
                principalTable: "POSMProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditReports_Products_ProductId",
                table: "AuditReports");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyCMActivities_Outlets_OutletId",
                table: "DailyCMActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_POSMReports_POSMProducts_ProductId",
                table: "POSMReports");            

            migrationBuilder.DropIndex(
                name: "IX_POSMReports_ProductId",
                table: "POSMReports");

            migrationBuilder.DropIndex(
                name: "IX_DailyCMActivities_OutletId",
                table: "DailyCMActivities");

            migrationBuilder.DropIndex(
                name: "IX_AuditReports_ProductId",
                table: "AuditReports");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");
        }
    }
}
