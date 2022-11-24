﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class dailyAVCommunicationTaskAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyCommunicationTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DailyTaskId = table.Column<int>(nullable: false),
                    TaskStatus = table.Column<int>(nullable: false),
                    Reason = table.Column<int>(nullable: false),
                    ReasonDetails = table.Column<string>(nullable: true),
                    CommunicationSetupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyCommunicationTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyCommunicationTasks_DailyTasks_DailyTaskId",
                        column: x => x.DailyTaskId,
                        principalTable: "DailyTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyCommunicationTasks_DailyTaskId",
                table: "DailyCommunicationTasks",
                column: "DailyTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyCommunicationTasks");
        }
    }
}
