using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class OrganizationUserRole_Modify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUserRoles_UserId",
                table: "OrganizationUserRoles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationUserRoles_UserInfos_UserId",
                table: "OrganizationUserRoles",
                column: "UserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationUserRoles_UserInfos_UserId",
                table: "OrganizationUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationUserRoles_UserId",
                table: "OrganizationUserRoles");
        }
    }
}
