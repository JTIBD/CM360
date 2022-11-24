using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class dailyTaskAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CmUserId = table.Column<int>(nullable: false),
                    SalesPointId = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyTasks_CMUsers_CmUserId",
                        column: x => x.CmUserId,
                        principalTable: "CMUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyTasks_SalesPoints_SalesPointId",
                        column: x => x.SalesPointId,
                        principalTable: "SalesPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyPosmTasks",
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
                    ExistingImage = table.Column<string>(nullable: true),
                    NewImage = table.Column<string>(nullable: true),
                    Reason = table.Column<int>(nullable: false),
                    ReasonDetails = table.Column<string>(nullable: true),
                    TaskStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPosmTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyPosmTasks_DailyTasks_DailyTaskId",
                        column: x => x.DailyTaskId,
                        principalTable: "DailyTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyPosmTaskItemses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DailyPosmTaskId = table.Column<int>(nullable: false),
                    PosmProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    WorkType = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPosmTaskItemses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyPosmTaskItemses_DailyPosmTasks_DailyPosmTaskId",
                        column: x => x.DailyPosmTaskId,
                        principalTable: "DailyPosmTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyPosmTaskItemses_POSMProducts_PosmProductId",
                        column: x => x.PosmProductId,
                        principalTable: "POSMProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyPosmTaskItemses_DailyPosmTaskId",
                table: "DailyPosmTaskItemses",
                column: "DailyPosmTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPosmTaskItemses_PosmProductId",
                table: "DailyPosmTaskItemses",
                column: "PosmProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPosmTasks_DailyTaskId",
                table: "DailyPosmTasks",
                column: "DailyTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTasks_CmUserId",
                table: "DailyTasks",
                column: "CmUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTasks_SalesPointId",
                table: "DailyTasks",
                column: "SalesPointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyPosmTaskItemses");

            migrationBuilder.DropTable(
                name: "DailyPosmTasks");

            migrationBuilder.DropTable(
                name: "DailyTasks");
        }
    }
}
