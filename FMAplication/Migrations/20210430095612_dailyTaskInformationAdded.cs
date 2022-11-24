using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class dailyTaskInformationAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkType",
                table: "DailyPosmTaskItemses");

            migrationBuilder.AddColumn<int>(
                name: "ExecutionType",
                table: "DailyPosmTaskItemses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DailyInformationTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    InsightImage = table.Column<string>(nullable: true),
                    InsightDescription = table.Column<string>(nullable: true),
                    RequestImage = table.Column<string>(nullable: true),
                    RequestDescription = table.Column<string>(nullable: true),
                    DailyTaskId = table.Column<int>(nullable: false),
                    OutletId = table.Column<int>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    Reason = table.Column<int>(nullable: false),
                    ReasonDetails = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyInformationTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyInformationTasks_DailyTasks_DailyTaskId",
                        column: x => x.DailyTaskId,
                        principalTable: "DailyTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyInformationTasks_DailyTaskId",
                table: "DailyInformationTasks",
                column: "DailyTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyInformationTasks");

            migrationBuilder.DropColumn(
                name: "ExecutionType",
                table: "DailyPosmTaskItemses");

            migrationBuilder.AddColumn<int>(
                name: "WorkType",
                table: "DailyPosmTaskItemses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
