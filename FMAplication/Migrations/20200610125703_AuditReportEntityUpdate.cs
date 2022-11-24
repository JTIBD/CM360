using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class AuditReportEntityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditReports_Products_ProductId",
                table: "AuditReports");

            migrationBuilder.AlterColumn<string>(
                name: "UploadedImageUrl2",
                table: "POSMReports",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UploadedImageUrl1",
                table: "POSMReports",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UploadedImageUrl2",
                table: "AuditReports",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UploadedImageUrl1",
                table: "AuditReports",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "AuditReports",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "POSMProductId",
                table: "AuditReports",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditReports_POSMProductId",
                table: "AuditReports",
                column: "POSMProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditReports_POSMProducts_POSMProductId",
                table: "AuditReports",
                column: "POSMProductId",
                principalTable: "POSMProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditReports_Products_ProductId",
                table: "AuditReports",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditReports_POSMProducts_POSMProductId",
                table: "AuditReports");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditReports_Products_ProductId",
                table: "AuditReports");

            migrationBuilder.DropIndex(
                name: "IX_AuditReports_POSMProductId",
                table: "AuditReports");

            migrationBuilder.DropColumn(
                name: "POSMProductId",
                table: "AuditReports");

            migrationBuilder.AlterColumn<string>(
                name: "UploadedImageUrl2",
                table: "POSMReports",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UploadedImageUrl1",
                table: "POSMReports",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UploadedImageUrl2",
                table: "AuditReports",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UploadedImageUrl1",
                table: "AuditReports",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "AuditReports",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditReports_Products_ProductId",
                table: "AuditReports",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
