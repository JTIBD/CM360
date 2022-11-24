using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class removeFKfromUserTerr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTerritoryMapping_Nodes_NodeId",
                table: "UserTerritoryMapping");

            migrationBuilder.DropIndex(
                name: "IX_UserTerritoryMapping_NodeId",
                table: "UserTerritoryMapping");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserTerritoryMapping_NodeId",
                table: "UserTerritoryMapping",
                column: "NodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTerritoryMapping_Nodes_NodeId",
                table: "UserTerritoryMapping",
                column: "NodeId",
                principalTable: "Nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
