using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class ActionTypeAddInProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActionType",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowLogHistories_UserId",
                table: "WorkflowLogHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowLogHistories_UserInfos_UserId",
                table: "WorkflowLogHistories",
                column: "UserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowLogHistories_UserInfos_UserId",
                table: "WorkflowLogHistories");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowLogHistories_UserId",
                table: "WorkflowLogHistories");

            migrationBuilder.DropColumn(
                name: "ActionType",
                table: "Products");
        }
    }
}
