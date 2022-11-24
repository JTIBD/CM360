using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class cmsuserTableModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AltCode",
                table: "CMUsers",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoiningDate",
                table: "CMUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NIdBirthCertificate",
                table: "CMUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "CMUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CmsUserSalesPointMappings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    CmUserId = table.Column<int>(nullable: false),
                    SalesPointId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmsUserSalesPointMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CmsUserSalesPointMappings_CMUsers_CmUserId",
                        column: x => x.CmUserId,
                        principalTable: "CMUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CmsUserSalesPointMappings_SalesPoints_SalesPointId",
                        column: x => x.SalesPointId,
                        principalTable: "SalesPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CmsUserSalesPointMappings_CmUserId",
                table: "CmsUserSalesPointMappings",
                column: "CmUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CmsUserSalesPointMappings_SalesPointId",
                table: "CmsUserSalesPointMappings",
                column: "SalesPointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CmsUserSalesPointMappings");

            migrationBuilder.DropColumn(
                name: "AltCode",
                table: "CMUsers");

            migrationBuilder.DropColumn(
                name: "JoiningDate",
                table: "CMUsers");

            migrationBuilder.DropColumn(
                name: "NIdBirthCertificate",
                table: "CMUsers");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "CMUsers");
        }
    }
}
