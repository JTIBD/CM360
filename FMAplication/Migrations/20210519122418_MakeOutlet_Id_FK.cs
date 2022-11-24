using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class MakeOutlet_Id_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DailyPosmTasks_OutletId",
                table: "DailyPosmTasks",
                column: "OutletId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyPosmTasks_Outlets_OutletId",
                table: "DailyPosmTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyPosmTasks_Outlets_OutletId",
                table: "DailyPosmTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyPosmTasks_OutletId",
                table: "DailyPosmTasks");
        }
    }
}
