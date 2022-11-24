using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class RemoveFK_SalesPoint_Setups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditSetups_SalesPoints_SalesPointId",
                table: "AuditSetups");

            migrationBuilder.DropForeignKey(
                name: "FK_AvSetups_SalesPoints_SalesPointId",
                table: "AvSetups");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunicationSetups_SalesPoints_SalesPointId",
                table: "CommunicationSetups");

            migrationBuilder.DropForeignKey(
                name: "FK_GuidelineSetups_SalesPoints_SalesPointId",
                table: "GuidelineSetups");

            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_SalesPoints_SalesPointId",
                table: "Surveys");

            migrationBuilder.DropIndex(
                name: "IX_Surveys_SalesPointId",
                table: "Surveys");

            migrationBuilder.DropIndex(
                name: "IX_GuidelineSetups_SalesPointId",
                table: "GuidelineSetups");

            migrationBuilder.DropIndex(
                name: "IX_CommunicationSetups_SalesPointId",
                table: "CommunicationSetups");

            migrationBuilder.DropIndex(
                name: "IX_AvSetups_SalesPointId",
                table: "AvSetups");

            migrationBuilder.DropIndex(
                name: "IX_AuditSetups_SalesPointId",
                table: "AuditSetups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Surveys_SalesPointId",
                table: "Surveys",
                column: "SalesPointId");

            migrationBuilder.CreateIndex(
                name: "IX_GuidelineSetups_SalesPointId",
                table: "GuidelineSetups",
                column: "SalesPointId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationSetups_SalesPointId",
                table: "CommunicationSetups",
                column: "SalesPointId");

            migrationBuilder.CreateIndex(
                name: "IX_AvSetups_SalesPointId",
                table: "AvSetups",
                column: "SalesPointId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditSetups_SalesPointId",
                table: "AuditSetups",
                column: "SalesPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditSetups_SalesPoints_SalesPointId",
                table: "AuditSetups",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AvSetups_SalesPoints_SalesPointId",
                table: "AvSetups",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationSetups_SalesPoints_SalesPointId",
                table: "CommunicationSetups",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GuidelineSetups_SalesPoints_SalesPointId",
                table: "GuidelineSetups",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_SalesPoints_SalesPointId",
                table: "Surveys",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
