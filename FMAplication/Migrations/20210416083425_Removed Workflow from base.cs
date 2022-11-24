using Microsoft.EntityFrameworkCore.Migrations;

namespace FMAplication.Migrations
{
    public partial class RemovedWorkflowfrombase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "WorkFlowTypes");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "WorkFlowTypes");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "WorkFlows");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "WorkFlows");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "WorkflowLogs");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "WorkflowLogs");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "WorkflowLogHistories");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "WorkflowLogHistories");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "WorkFlowConfigurations");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "WDistributionTransactions");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "WDistributionTransactions");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "WDistributionRecieveTransactions");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "WDistributionRecieveTransactions");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "WareHouseStocks");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "WareHouseStocks");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "WareHouses");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "WareHouses");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "UserTerritoryMapping");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "UserTerritoryMapping");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "UserRoleMapping");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "UserRoleMapping");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "SurveyReports");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "SurveyReports");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "SurveyQuestionSets");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "SurveyQuestionSets");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "SurveyQuestionCollections");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "SurveyQuestionCollections");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "SubBrands");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "SubBrands");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "StockAdjustmentItems");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "StockAdjustmentItems");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "StockAddTransactions");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "StockAddTransactions");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "SalesPointStocks");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "SalesPointStocks");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "SalesPoints");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "SalesPoints");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "SalesPointNodeMapping");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "SalesPointNodeMapping");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "PosmTaskAssigns");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "PosmTaskAssigns");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "POSMReports");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "POSMReports");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "POSMProducts");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "POSMProducts");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "OrganizationUserRoles");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "OrganizationUserRoles");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "OrganizationRoles");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "OrganizationRoles");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "MenuPermissions");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "MenuPermissions");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "MenuActivityPermissions");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "MenuActivityPermissions");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "MenuActivities");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "MenuActivities");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Hierarchy");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Hierarchy");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Examples");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Examples");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Delegations");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Delegations");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "DailyPOSMs");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "DailyPOSMs");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "DailyCMActivities");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "DailyCMActivities");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "DailyAudits");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "DailyAudits");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "CMUsers");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "CMUsers");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "CmsUserSalesPointMappings");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "CmsUserSalesPointMappings");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "CampaignHistories");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "CampaignHistories");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "AuditReports");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "AuditReports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "WorkFlowTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "WorkFlowTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "WorkFlows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "WorkFlows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "WorkflowLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "WorkflowLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "WorkflowLogHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "WorkflowLogHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "WorkFlowConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "WorkFlowConfigurations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "WDistributionTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "WDistributionTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "WDistributionRecieveTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "WDistributionRecieveTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "WareHouseStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "WareHouseStocks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "WareHouses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "WareHouses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "UserTerritoryMapping",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "UserTerritoryMapping",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "UserRoleMapping",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "UserRoleMapping",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "UserInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "UserInfos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Surveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Surveys",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "SurveyReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "SurveyReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "SurveyQuestionSets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "SurveyQuestionSets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "SurveyQuestionCollections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "SurveyQuestionCollections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "SubBrands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "SubBrands",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "StockAdjustmentItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "StockAdjustmentItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "StockAddTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "StockAddTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "SalesPointStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "SalesPointStocks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "SalesPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "SalesPoints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "SalesPointNodeMapping",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "SalesPointNodeMapping",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Routes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "QuestionOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "QuestionOptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "PosmTaskAssigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "PosmTaskAssigns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "POSMReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "POSMReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "POSMProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "POSMProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Outlets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Outlets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "OrganizationUserRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "OrganizationUserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "OrganizationRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "OrganizationRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Nodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Nodes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Menus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Menus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "MenuPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "MenuPermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "MenuActivityPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "MenuActivityPermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "MenuActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "MenuActivities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Hierarchy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Hierarchy",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Examples",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Examples",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Delegations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Delegations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "DailyPOSMs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "DailyPOSMs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "DailyCMActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "DailyCMActivities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "DailyAudits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "DailyAudits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "CMUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "CMUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "CmsUserSalesPointMappings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "CmsUserSalesPointMappings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Channels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Channels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Campaigns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "CampaignHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "CampaignHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Brands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Brands",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "AuditReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "AuditReports",
                type: "int",
                nullable: true);
        }
    }
}
