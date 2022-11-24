using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class Table_AvSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvSetups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    SalesPointId = table.Column<int>(nullable: false),
                    AvId = table.Column<int>(nullable: false),
                    UserType = table.Column<int>(nullable: false),
                    fromDate = table.Column<DateTime>(nullable: false),
                    toDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvSetups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvSetups_AvCommunications_AvId",
                        column: x => x.AvId,
                        principalTable: "AvCommunications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvSetups_SalesPoints_SalesPointId",
                        column: x => x.SalesPointId,
                        principalTable: "SalesPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvSetups_AvId",
                table: "AvSetups",
                column: "AvId");

            migrationBuilder.CreateIndex(
                name: "IX_AvSetups_SalesPointId",
                table: "AvSetups",
                column: "SalesPointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvSetups");
        }
    }
}
