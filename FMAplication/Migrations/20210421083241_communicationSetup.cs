using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class communicationSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunicationSetups",
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
                    AvCommunicationId = table.Column<int>(nullable: false),
                    UserType = table.Column<int>(nullable: false),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationSetups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationSetups_AvCommunications_AvCommunicationId",
                        column: x => x.AvCommunicationId,
                        principalTable: "AvCommunications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunicationSetups_SalesPoints_SalesPointId",
                        column: x => x.SalesPointId,
                        principalTable: "SalesPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationSetups_AvCommunicationId",
                table: "CommunicationSetups",
                column: "AvCommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationSetups_SalesPointId",
                table: "CommunicationSetups",
                column: "SalesPointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunicationSetups");
        }
    }
}
