using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class AddInitialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Questions_QuestionTitle",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SurveyQuestionCollections");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "POSMProducts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Examples");

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Surveys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Surveys",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SurveyQuestionCollections",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "SurveyQuestionCollections",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "SurveyQuestionCollections",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "POSMProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "POSMProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "POSMProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Examples",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Examples",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Examples",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    DailyCMActivityId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    ActionType = table.Column<int>(nullable: false),
                    UploadedImageUrl1 = table.Column<string>(maxLength: 512, nullable: true),
                    UploadedImageUrl2 = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyAudits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    DailyCMActivityId = table.Column<int>(nullable: false),
                    IsDistributionCheck = table.Column<bool>(nullable: false),
                    IsFacingCount = table.Column<bool>(nullable: false),
                    IsPlanogramCheck = table.Column<bool>(nullable: false),
                    IsPriceAudit = table.Column<bool>(nullable: false),
                    DistributionCheckStatus = table.Column<int>(nullable: false),
                    FacingCountStatus = table.Column<int>(nullable: false),
                    PlanogramCheckStatus = table.Column<int>(nullable: false),
                    PriceAuditCheckStatus = table.Column<string>(nullable: true),
                    DistributionCheckIncompleteReason = table.Column<string>(maxLength: 512, nullable: true),
                    FacingCountCheckIncompleteReason = table.Column<string>(maxLength: 512, nullable: true),
                    PlanogramCheckIncompleteReason = table.Column<string>(maxLength: 512, nullable: true),
                    PriceAuditCheckIncompleteReason = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyAudits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyCMActivities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    OutletId = table.Column<int>(nullable: false),
                    CMId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    AssignedFMUserId = table.Column<int>(nullable: false),
                    IsAutid = table.Column<bool>(nullable: false),
                    IsSurvey = table.Column<bool>(nullable: false),
                    IsPOSM = table.Column<bool>(nullable: false),
                    IsConsumerSurveyActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyCMActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyPOSMs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    IsPOSMInstallation = table.Column<bool>(nullable: false),
                    IsPOSMRepair = table.Column<bool>(nullable: false),
                    IsPOSMRemoval = table.Column<bool>(nullable: false),
                    DailyCMActivityId = table.Column<int>(nullable: false),
                    POSMInstallationStatus = table.Column<int>(nullable: false),
                    POSMRepairStatus = table.Column<int>(nullable: false),
                    POSMRemovalStatus = table.Column<int>(nullable: false),
                    POSMRemovalIncompleteReason = table.Column<string>(maxLength: 512, nullable: true),
                    POSMInstallationIncompleteReason = table.Column<string>(maxLength: 512, nullable: true),
                    POSMRepairIncompleteReason = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPOSMs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Controller = table.Column<string>(maxLength: 128, nullable: true),
                    Action = table.Column<string>(maxLength: 128, nullable: true),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Menus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    NodeId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Outlets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    OutletId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    BanglaName = table.Column<string>(maxLength: 256, nullable: true),
                    OutletNameBangla = table.Column<string>(maxLength: 256, nullable: true),
                    OwnerName = table.Column<string>(maxLength: 256, nullable: true),
                    OwnerNameBangla = table.Column<string>(maxLength: 256, nullable: true),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    AddressBangla = table.Column<string>(maxLength: 500, nullable: true),
                    ContactNumber = table.Column<string>(maxLength: 128, nullable: true),
                    Location = table.Column<string>(maxLength: 128, nullable: true),
                    Latitude = table.Column<string>(maxLength: 128, nullable: true),
                    Longitude = table.Column<string>(maxLength: 128, nullable: true),
                    SalesPointId = table.Column<int>(nullable: false),
                    RouteId = table.Column<int>(nullable: false),
                    NodeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outlets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "POSMReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    DailyCMActivityId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    ActionType = table.Column<int>(nullable: false),
                    UploadedImageUrl1 = table.Column<string>(maxLength: 512, nullable: true),
                    UploadedImageUrl2 = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POSMReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    OptionTitle = table.Column<string>(maxLength: 256, nullable: false),
                    Sequence = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    RouteId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 128, nullable: true),
                    RouteName = table.Column<string>(maxLength: 256, nullable: false),
                    RouteNameBangla = table.Column<string>(maxLength: 256, nullable: true),
                    NumberOfOutlets = table.Column<int>(nullable: false),
                    ContactNo = table.Column<string>(maxLength: 128, nullable: true),
                    SalesPointId = table.Column<int>(nullable: false),
                    NodeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesPointNodeMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    NodeId = table.Column<int>(nullable: false),
                    SalesPointId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPointNodeMapping", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    SalesPointId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    BanglaName = table.Column<string>(maxLength: 256, nullable: true),
                    OfficeAddress = table.Column<string>(maxLength: 500, nullable: true),
                    ContactNo = table.Column<string>(maxLength: 500, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 128, nullable: true),
                    DeliveryChannelType = table.Column<int>(nullable: false),
                    Latitude = table.Column<string>(maxLength: 128, nullable: true),
                    Longitude = table.Column<string>(maxLength: 128, nullable: true),
                    SequenceId = table.Column<int>(nullable: false),
                    TownName = table.Column<string>(maxLength: 128, nullable: true),
                    DepotCode = table.Column<int>(nullable: false),
                    OfficeAddressBangla = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    DailyActivityId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    SurveyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<string>(maxLength: 128, nullable: false),
                    Code = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 128, nullable: true),
                    Designation = table.Column<string>(maxLength: 256, nullable: true),
                    SalesPointId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Action = table.Column<string>(maxLength: 128, nullable: true),
                    WorkflowType = table.Column<int>(nullable: false),
                    WorkflowStep = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuActivities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    MenuId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuActivities_Menus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationUserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    OrgRoleId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationUserRoles_OrganizationRoles_OrgRoleId",
                        column: x => x.OrgRoleId,
                        principalTable: "OrganizationRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    MenuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuPermissions_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuPermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CMUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 128, nullable: true),
                    Email = table.Column<string>(maxLength: 128, nullable: true),
                    Password = table.Column<string>(maxLength: 256, nullable: true),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    FamilyContactNo = table.Column<string>(maxLength: 500, nullable: true),
                    FMUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CMUsers_UserInfos_FMUserId",
                        column: x => x.FMUserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    UserInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoleMapping_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleMapping_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    OrgRoleId = table.Column<int>(nullable: false),
                    MasterWorkFlowId = table.Column<int>(nullable: false),
                    ModeOfApproval = table.Column<int>(nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false),
                    ReceivedStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkFlowConfigurations_WorkFlows_MasterWorkFlowId",
                        column: x => x.MasterWorkFlowId,
                        principalTable: "WorkFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    RowId = table.Column<int>(nullable: false),
                    WorkFlowFor = table.Column<int>(nullable: false),
                    MasterWorkFlowId = table.Column<int>(nullable: false),
                    WorkflowStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowLogs_WorkFlows_MasterWorkFlowId",
                        column: x => x.MasterWorkFlowId,
                        principalTable: "WorkFlows",
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
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
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

            migrationBuilder.CreateTable(
                name: "WorkflowLogHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    WorkflowLogId = table.Column<int>(nullable: false),
                    WorkflowStatus = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowLogHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowLogHistories_WorkflowLogs_WorkflowLogId",
                        column: x => x.WorkflowLogId,
                        principalTable: "WorkflowLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CMUsers_FMUserId",
                table: "CMUsers",
                column: "FMUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuActivities_ParentId",
                table: "MenuActivities",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuActivityPermissions_ActivityId",
                table: "MenuActivityPermissions",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuActivityPermissions_RoleId",
                table: "MenuActivityPermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermissions_MenuId",
                table: "MenuPermissions",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermissions_RoleId",
                table: "MenuPermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_ParentId",
                table: "Menus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRoles_Name",
                table: "OrganizationRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUserRoles_OrgRoleId",
                table: "OrganizationUserRoles",
                column: "OrgRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_OptionTitle",
                table: "QuestionOptions",
                column: "OptionTitle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_Code",
                table: "UserInfos",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMapping_RoleId",
                table: "UserRoleMapping",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMapping_UserInfoId",
                table: "UserRoleMapping",
                column: "UserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowConfigurations_MasterWorkFlowId",
                table: "WorkFlowConfigurations",
                column: "MasterWorkFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowLogHistories_WorkflowLogId",
                table: "WorkflowLogHistories",
                column: "WorkflowLogId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowLogs_MasterWorkFlowId",
                table: "WorkflowLogs",
                column: "MasterWorkFlowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditReports");

            migrationBuilder.DropTable(
                name: "CMUsers");

            migrationBuilder.DropTable(
                name: "DailyAudits");

            migrationBuilder.DropTable(
                name: "DailyCMActivities");

            migrationBuilder.DropTable(
                name: "DailyPOSMs");

            migrationBuilder.DropTable(
                name: "MenuActivityPermissions");

            migrationBuilder.DropTable(
                name: "MenuPermissions");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "OrganizationUserRoles");

            migrationBuilder.DropTable(
                name: "Outlets");

            migrationBuilder.DropTable(
                name: "POSMReports");

            migrationBuilder.DropTable(
                name: "QuestionOptions");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "SalesPointNodeMapping");

            migrationBuilder.DropTable(
                name: "SalesPoints");

            migrationBuilder.DropTable(
                name: "SurveyReports");

            migrationBuilder.DropTable(
                name: "UserRoleMapping");

            migrationBuilder.DropTable(
                name: "WorkFlowConfigurations");

            migrationBuilder.DropTable(
                name: "WorkflowLogHistories");

            migrationBuilder.DropTable(
                name: "MenuActivities");

            migrationBuilder.DropTable(
                name: "OrganizationRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserInfos");

            migrationBuilder.DropTable(
                name: "WorkflowLogs");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "WorkFlows");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SurveyQuestionCollections");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "SurveyQuestionCollections");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "SurveyQuestionCollections");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "POSMProducts");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "POSMProducts");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "POSMProducts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Examples");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Examples");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Examples");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Surveys",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SurveyQuestionCollections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "POSMProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Examples",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTitle",
                table: "Questions",
                column: "QuestionTitle",
                unique: true);
        }
    }
}
