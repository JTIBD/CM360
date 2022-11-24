using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class UserTerritoryMappingAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesPointId",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "TerritoryNodeId",
                table: "UserInfos");

            migrationBuilder.CreateTable(
                name: "UserTerritoryMapping",
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
                    NodeId = table.Column<int>(nullable: false),
                    UserInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTerritoryMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTerritoryMapping_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTerritoryMapping_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTerritoryMapping_NodeId",
                table: "UserTerritoryMapping",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTerritoryMapping_UserInfoId",
                table: "UserTerritoryMapping",
                column: "UserInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTerritoryMapping");

            migrationBuilder.AddColumn<int>(
                name: "SalesPointId",
                table: "UserInfos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TerritoryNodeId",
                table: "UserInfos",
                type: "int",
                nullable: true);
        }
    }
}
