using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class MenuActivityAndPermissionTableAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuActivities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    ActivityCode = table.Column<string>(nullable: true),
                    MenuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuActivities_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuActivityPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    CanView = table.Column<bool>(nullable: false),
                    CanUpdate = table.Column<bool>(nullable: false),
                    CanInsert = table.Column<bool>(nullable: false),
                    CanDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuActivityPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuActivityPermissions_MenuActivities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "MenuActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuActivityPermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuActivities_ActivityCode",
                table: "MenuActivities",
                column: "ActivityCode",
                unique: true,
                filter: "[ActivityCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MenuActivities_MenuId",
                table: "MenuActivities",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuActivities_Name",
                table: "MenuActivities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuActivityPermissions_RoleId",
                table: "MenuActivityPermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuActivityPermissions_ActivityId_RoleId",
                table: "MenuActivityPermissions",
                columns: new[] { "ActivityId", "RoleId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuActivityPermissions");

            migrationBuilder.DropTable(
                name: "MenuActivities");
        }
    }
}
