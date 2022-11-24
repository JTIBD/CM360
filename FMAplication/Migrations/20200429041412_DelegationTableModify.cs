using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class DelegationTableModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Delegations_DeligatedFromUserId",
                table: "Delegations",
                column: "DeligatedFromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegations_DeligatedToUserId",
                table: "Delegations",
                column: "DeligatedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Delegations_UserInfos_DeligatedFromUserId",
                table: "Delegations",
                column: "DeligatedFromUserId",
                principalTable: "UserInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Delegations_UserInfos_DeligatedToUserId",
                table: "Delegations",
                column: "DeligatedToUserId",
                principalTable: "UserInfos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delegations_UserInfos_DeligatedFromUserId",
                table: "Delegations");

            migrationBuilder.DropForeignKey(
                name: "FK_Delegations_UserInfos_DeligatedToUserId",
                table: "Delegations");

            migrationBuilder.DropIndex(
                name: "IX_Delegations_DeligatedFromUserId",
                table: "Delegations");

            migrationBuilder.DropIndex(
                name: "IX_Delegations_DeligatedToUserId",
                table: "Delegations");
        }
    }
}
