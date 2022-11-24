using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class ProductPOSMProductUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubBrandId",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "POSMProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "POSMProducts",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "SubBrandId",
                table: "POSMProducts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubBrandId",
                table: "Products",
                column: "SubBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_POSMProducts_BrandId",
                table: "POSMProducts",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_POSMProducts_CampaignId",
                table: "POSMProducts",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_POSMProducts_SubBrandId",
                table: "POSMProducts",
                column: "SubBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_POSMProducts_Brands_BrandId",
                table: "POSMProducts",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POSMProducts_Campaigns_CampaignId",
                table: "POSMProducts",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_POSMProducts_SubBrands_SubBrandId",
                table: "POSMProducts",
                column: "SubBrandId",
                principalTable: "SubBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubBrands_SubBrandId",
                table: "Products",
                column: "SubBrandId",
                principalTable: "SubBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POSMProducts_Brands_BrandId",
                table: "POSMProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_POSMProducts_Campaigns_CampaignId",
                table: "POSMProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_POSMProducts_SubBrands_SubBrandId",
                table: "POSMProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubBrands_SubBrandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SubBrandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_POSMProducts_BrandId",
                table: "POSMProducts");

            migrationBuilder.DropIndex(
                name: "IX_POSMProducts_CampaignId",
                table: "POSMProducts");

            migrationBuilder.DropIndex(
                name: "IX_POSMProducts_SubBrandId",
                table: "POSMProducts");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubBrandId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "POSMProducts");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "POSMProducts");

            migrationBuilder.DropColumn(
                name: "SubBrandId",
                table: "POSMProducts");
        }
    }
}
