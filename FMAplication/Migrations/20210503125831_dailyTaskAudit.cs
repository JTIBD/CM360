using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class dailyTaskAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Reason",
                table: "DailySurveyTasks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Reason",
                table: "DailyPosmTasks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Reason",
                table: "DailyInformationTasks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Reason",
                table: "DailyCommunicationTasks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Reason",
                table: "DailyAvTasks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "DailyAuditTasks",
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
                    AuditSetupId = table.Column<int>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    Reason = table.Column<int>(nullable: true),
                    ReasonDetails = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyAuditTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyAuditTasks_DailyTasks_DailyTaskId",
                        column: x => x.DailyTaskId,
                        principalTable: "DailyTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyPosmAuditTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    PosmProductId = table.Column<int>(nullable: false),
                    DailyAuditTaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPosmAuditTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyPosmAuditTasks_DailyAuditTasks_DailyAuditTaskId",
                        column: x => x.DailyAuditTaskId,
                        principalTable: "DailyAuditTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyPosmAuditTasks_POSMProducts_PosmProductId",
                        column: x => x.PosmProductId,
                        principalTable: "POSMProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyProductsAuditTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    DailyAuditTaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyProductsAuditTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyProductsAuditTasks_DailyAuditTasks_DailyAuditTaskId",
                        column: x => x.DailyAuditTaskId,
                        principalTable: "DailyAuditTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyProductsAuditTasks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyAuditTasks_DailyTaskId",
                table: "DailyAuditTasks",
                column: "DailyTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPosmAuditTasks_DailyAuditTaskId",
                table: "DailyPosmAuditTasks",
                column: "DailyAuditTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPosmAuditTasks_PosmProductId",
                table: "DailyPosmAuditTasks",
                column: "PosmProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyProductsAuditTasks_DailyAuditTaskId",
                table: "DailyProductsAuditTasks",
                column: "DailyAuditTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyProductsAuditTasks_ProductId",
                table: "DailyProductsAuditTasks",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyPosmAuditTasks");

            migrationBuilder.DropTable(
                name: "DailyProductsAuditTasks");

            migrationBuilder.DropTable(
                name: "DailyAuditTasks");

            migrationBuilder.AlterColumn<int>(
                name: "Reason",
                table: "DailySurveyTasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Reason",
                table: "DailyPosmTasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Reason",
                table: "DailyInformationTasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Reason",
                table: "DailyCommunicationTasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Reason",
                table: "DailyAvTasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
