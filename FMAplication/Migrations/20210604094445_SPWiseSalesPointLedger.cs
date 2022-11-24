using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class SPWiseSalesPointLedger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpWisePosmLedgers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    PosmProductId = table.Column<int>(nullable: true),
                    PosmProuctId = table.Column<int>(nullable: false),
                    SalesPointId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    OpeningStock = table.Column<int>(nullable: false),
                    ReceivedStock = table.Column<int>(nullable: false),
                    ExecutedStock = table.Column<int>(nullable: false),
                    ClosingStock = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpWisePosmLedgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpWisePosmLedgers_POSMProducts_PosmProductId",
                        column: x => x.PosmProductId,
                        principalTable: "POSMProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpWisePosmLedgers_PosmProductId",
                table: "SpWisePosmLedgers",
                column: "PosmProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpWisePosmLedgers");
        }
    }
}
