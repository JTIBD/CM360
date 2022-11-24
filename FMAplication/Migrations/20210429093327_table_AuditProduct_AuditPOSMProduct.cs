using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class table_AuditProduct_AuditPOSMProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditPOSMProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    AuditSetupId = table.Column<int>(nullable: false),
                    POSMProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditPOSMProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditPOSMProducts_AuditSetups_AuditSetupId",
                        column: x => x.AuditSetupId,
                        principalTable: "AuditSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditPOSMProducts_POSMProducts_POSMProductId",
                        column: x => x.POSMProductId,
                        principalTable: "POSMProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    AuditSetupId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditProducts_AuditSetups_AuditSetupId",
                        column: x => x.AuditSetupId,
                        principalTable: "AuditSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditPOSMProducts_AuditSetupId",
                table: "AuditPOSMProducts",
                column: "AuditSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPOSMProducts_POSMProductId",
                table: "AuditPOSMProducts",
                column: "POSMProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProducts_AuditSetupId",
                table: "AuditProducts",
                column: "AuditSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProducts_ProductId",
                table: "AuditProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditPOSMProducts");

            migrationBuilder.DropTable(
                name: "AuditProducts");
        }
    }
}
