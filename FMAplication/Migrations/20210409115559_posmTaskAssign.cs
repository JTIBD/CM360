using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class posmTaskAssign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PosmTaskAssigns",
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
                    SalesPointId = table.Column<int>(nullable: false),
                    PosmProductId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CmUserId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosmTaskAssigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosmTaskAssigns_CMUsers_CmUserId",
                        column: x => x.CmUserId,
                        principalTable: "CMUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmTaskAssigns_POSMProducts_PosmProductId",
                        column: x => x.PosmProductId,
                        principalTable: "POSMProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PosmTaskAssigns_SalesPoints_SalesPointId",
                        column: x => x.SalesPointId,
                        principalTable: "SalesPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PosmTaskAssigns_CmUserId",
                table: "PosmTaskAssigns",
                column: "CmUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmTaskAssigns_PosmProductId",
                table: "PosmTaskAssigns",
                column: "PosmProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmTaskAssigns_SalesPointId",
                table: "PosmTaskAssigns",
                column: "SalesPointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PosmTaskAssigns");
        }
    }
}
