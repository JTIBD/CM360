using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class RemoveFK_Outlet_DailyTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyAuditTasks_Outlets_OutletId",
                table: "DailyAuditTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyAvTasks_Outlets_OutletId",
                table: "DailyAvTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyCommunicationTasks_Outlets_OutletId",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyConsumerSurveyTasks_Outlets_OutletId",
                table: "DailyConsumerSurveyTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyInformationTasks_Outlets_OutletId",
                table: "DailyInformationTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyPosmTasks_Outlets_OutletId",
                table: "DailyPosmTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DailySurveyTasks_Outlets_OutletId",
                table: "DailySurveyTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailySurveyTasks_OutletId",
                table: "DailySurveyTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyPosmTasks_OutletId",
                table: "DailyPosmTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyInformationTasks_OutletId",
                table: "DailyInformationTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyConsumerSurveyTasks_OutletId",
                table: "DailyConsumerSurveyTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyCommunicationTasks_OutletId",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyAvTasks_OutletId",
                table: "DailyAvTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyAuditTasks_OutletId",
                table: "DailyAuditTasks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DailySurveyTasks_OutletId",
                table: "DailySurveyTasks",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPosmTasks_OutletId",
                table: "DailyPosmTasks",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyInformationTasks_OutletId",
                table: "DailyInformationTasks",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyConsumerSurveyTasks_OutletId",
                table: "DailyConsumerSurveyTasks",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyCommunicationTasks_OutletId",
                table: "DailyCommunicationTasks",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAvTasks_OutletId",
                table: "DailyAvTasks",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAuditTasks_OutletId",
                table: "DailyAuditTasks",
                column: "OutletId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyAuditTasks_Outlets_OutletId",
                table: "DailyAuditTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyAvTasks_Outlets_OutletId",
                table: "DailyAvTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyCommunicationTasks_Outlets_OutletId",
                table: "DailyCommunicationTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyConsumerSurveyTasks_Outlets_OutletId",
                table: "DailyConsumerSurveyTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyInformationTasks_Outlets_OutletId",
                table: "DailyInformationTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyPosmTasks_Outlets_OutletId",
                table: "DailyPosmTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailySurveyTasks_Outlets_OutletId",
                table: "DailySurveyTasks",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
