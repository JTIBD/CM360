using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class posmTaskAssignUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PosmTaskAssigns_CMUsers_CmUserId",
                table: "PosmTaskAssigns");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PosmTaskAssigns");

            migrationBuilder.AlterColumn<int>(
                name: "CmUserId",
                table: "PosmTaskAssigns",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDownloaded",
                table: "PosmTaskAssigns",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_PosmTaskAssigns_CMUsers_CmUserId",
                table: "PosmTaskAssigns",
                column: "CmUserId",
                principalTable: "CMUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PosmTaskAssigns_CMUsers_CmUserId",
                table: "PosmTaskAssigns");

            migrationBuilder.DropColumn(
                name: "IsDownloaded",
                table: "PosmTaskAssigns");

            migrationBuilder.AlterColumn<int>(
                name: "CmUserId",
                table: "PosmTaskAssigns",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PosmTaskAssigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PosmTaskAssigns_CMUsers_CmUserId",
                table: "PosmTaskAssigns",
                column: "CmUserId",
                principalTable: "CMUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
