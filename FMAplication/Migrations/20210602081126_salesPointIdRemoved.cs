using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class salesPointIdRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CmsUserSalesPointMappings_SalesPoints_SalesPointId",
                table: "CmsUserSalesPointMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_PosmTaskAssigns_SalesPoints_SalesPointId",
                table: "PosmTaskAssigns");

            migrationBuilder.DropIndex(
                name: "IX_PosmTaskAssigns_SalesPointId",
                table: "PosmTaskAssigns");

            migrationBuilder.DropIndex(
                name: "IX_CmsUserSalesPointMappings_SalesPointId",
                table: "CmsUserSalesPointMappings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PosmTaskAssigns_SalesPointId",
                table: "PosmTaskAssigns",
                column: "SalesPointId");

            migrationBuilder.CreateIndex(
                name: "IX_CmsUserSalesPointMappings_SalesPointId",
                table: "CmsUserSalesPointMappings",
                column: "SalesPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_CmsUserSalesPointMappings_SalesPoints_SalesPointId",
                table: "CmsUserSalesPointMappings",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PosmTaskAssigns_SalesPoints_SalesPointId",
                table: "PosmTaskAssigns",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
