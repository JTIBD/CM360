using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class DailyTaskReasonUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "DailySurveyTasks");

            migrationBuilder.DropColumn(
                name: "ReasonDetails",
                table: "DailySurveyTasks");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "DailyPosmTasks");

            migrationBuilder.DropColumn(
                name: "ReasonDetails",
                table: "DailyPosmTasks");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "DailyInformationTasks");

            migrationBuilder.DropColumn(
                name: "ReasonDetails",
                table: "DailyInformationTasks");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "DailyConsumerSurveyTasks");

            migrationBuilder.DropColumn(
                name: "ReasonDetails",
                table: "DailyConsumerSurveyTasks");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropColumn(
                name: "ReasonDetails",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "DailyAvTasks");

            migrationBuilder.DropColumn(
                name: "ReasonDetails",
                table: "DailyAvTasks");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "DailyAuditTasks");

            migrationBuilder.DropColumn(
                name: "ReasonDetails",
                table: "DailyAuditTasks");

            migrationBuilder.AddColumn<int>(
                name: "ReasonId",
                table: "DailySurveyTasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReasonId",
                table: "DailyPosmTasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReasonId",
                table: "DailyInformationTasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReasonId",
                table: "DailyConsumerSurveyTasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReasonId",
                table: "DailyCommunicationTasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReasonId",
                table: "DailyAvTasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReasonId",
                table: "DailyAuditTasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailySurveyTasks_ReasonId",
                table: "DailySurveyTasks",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPosmTasks_ReasonId",
                table: "DailyPosmTasks",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyInformationTasks_ReasonId",
                table: "DailyInformationTasks",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyConsumerSurveyTasks_ReasonId",
                table: "DailyConsumerSurveyTasks",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyCommunicationTasks_ReasonId",
                table: "DailyCommunicationTasks",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAvTasks_ReasonId",
                table: "DailyAvTasks",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyAuditTasks_ReasonId",
                table: "DailyAuditTasks",
                column: "ReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyAuditTasks_Reasons_ReasonId",
                table: "DailyAuditTasks",
                column: "ReasonId",
                principalTable: "Reasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyAvTasks_Reasons_ReasonId",
                table: "DailyAvTasks",
                column: "ReasonId",
                principalTable: "Reasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyCommunicationTasks_Reasons_ReasonId",
                table: "DailyCommunicationTasks",
                column: "ReasonId",
                principalTable: "Reasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyConsumerSurveyTasks_Reasons_ReasonId",
                table: "DailyConsumerSurveyTasks",
                column: "ReasonId",
                principalTable: "Reasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyInformationTasks_Reasons_ReasonId",
                table: "DailyInformationTasks",
                column: "ReasonId",
                principalTable: "Reasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyPosmTasks_Reasons_ReasonId",
                table: "DailyPosmTasks",
                column: "ReasonId",
                principalTable: "Reasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DailySurveyTasks_Reasons_ReasonId",
                table: "DailySurveyTasks",
                column: "ReasonId",
                principalTable: "Reasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyAuditTasks_Reasons_ReasonId",
                table: "DailyAuditTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyAvTasks_Reasons_ReasonId",
                table: "DailyAvTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyCommunicationTasks_Reasons_ReasonId",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyConsumerSurveyTasks_Reasons_ReasonId",
                table: "DailyConsumerSurveyTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyInformationTasks_Reasons_ReasonId",
                table: "DailyInformationTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyPosmTasks_Reasons_ReasonId",
                table: "DailyPosmTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_DailySurveyTasks_Reasons_ReasonId",
                table: "DailySurveyTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailySurveyTasks_ReasonId",
                table: "DailySurveyTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyPosmTasks_ReasonId",
                table: "DailyPosmTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyInformationTasks_ReasonId",
                table: "DailyInformationTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyConsumerSurveyTasks_ReasonId",
                table: "DailyConsumerSurveyTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyCommunicationTasks_ReasonId",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyAvTasks_ReasonId",
                table: "DailyAvTasks");

            migrationBuilder.DropIndex(
                name: "IX_DailyAuditTasks_ReasonId",
                table: "DailyAuditTasks");

            migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "DailySurveyTasks");

            migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "DailyPosmTasks");

            migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "DailyInformationTasks");

            migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "DailyConsumerSurveyTasks");

            migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "DailyCommunicationTasks");

            migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "DailyAvTasks");

            migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "DailyAuditTasks");

            migrationBuilder.AddColumn<int>(
                name: "Reason",
                table: "DailySurveyTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonDetails",
                table: "DailySurveyTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Reason",
                table: "DailyPosmTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonDetails",
                table: "DailyPosmTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Reason",
                table: "DailyInformationTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonDetails",
                table: "DailyInformationTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Reason",
                table: "DailyConsumerSurveyTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonDetails",
                table: "DailyConsumerSurveyTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Reason",
                table: "DailyCommunicationTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonDetails",
                table: "DailyCommunicationTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Reason",
                table: "DailyAvTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonDetails",
                table: "DailyAvTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Reason",
                table: "DailyAuditTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonDetails",
                table: "DailyAuditTasks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
