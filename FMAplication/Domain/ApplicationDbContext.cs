using FMAplication.Domain.Audit;
using FMAplication.Domain.AVCommunications;
using FMAplication.Domain.Brand;
using FMAplication.Domain.Campaign;
using FMAplication.Domain.DailyActivities;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Examples;
using FMAplication.Domain.ExecutionLimits;
using FMAplication.Domain.Guidelines;
using FMAplication.Domain.Menus;
using FMAplication.Domain.Organizations;
using FMAplication.Domain.Products;
using FMAplication.Domain.Questions;
using FMAplication.Domain.Reports;
using FMAplication.Domain.Sales;
using FMAplication.Domain.SPWisePOSMLedgers;
using FMAplication.Domain.SyncInformations;
using FMAplication.Domain.Task;
using FMAplication.Domain.TransactionWorkFlows;
using FMAplication.Domain.Users;
using FMAplication.Domain.WareHouse;
using FMAplication.Domain.WorkFlows;
using FMAplication.Extensions;
using FMAplication.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FMAplication.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public static ApplicationDbContext Create(DbContextOptions<ApplicationDbContext> options)
        {
            return new ApplicationDbContext(options);
        }
        public static ApplicationDbContext Create()
        {
            DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(AppSettingsJson.ConnectionString);
            return Create(builder.Options);
        }


        #region Examples
        public DbSet<Example> Examples { get; set; }

        #endregion

        #region Daily Activities
        public DbSet<DailyAudit> DailyAudits { get; set; }
        public DbSet<DailyCMActivity> DailyCMActivities { get; set; }
        public DbSet<DailyPOSM> DailyPOSMs { get; set; }

        #endregion

        #region Menus
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuActivity> MenuActivities { get; set; }
        public DbSet<MenuActivityPermission> MenuActivityPermissions { get; set; }
        public DbSet<MenuPermission> MenuPermissions { get; set; }

        #endregion

        #region Organizations
        public DbSet<OrganizationRole> OrganizationRoles { get; set; }
        public DbSet<OrganizationUserRole> OrganizationUserRoles { get; set; }

        #endregion

        #region Product
        public DbSet<Product> Products { get; set; }
        public DbSet<POSMProduct> POSMProducts { get; set; }
        #endregion

        #region SurveyQuestion

        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<SurveyQuestionSet> SurveyQuestionSets { get; set; }
        public DbSet<SurveyQuestionCollection> SurveyQuestionCollections { get; set; }
        #endregion

        #region Reports
        public DbSet<AuditReport> AuditReports { get; set; }
        public DbSet<POSMReport> POSMReports { get; set; }
        public DbSet<SurveyReport> SurveyReports { get; set; }

        #endregion

        #region Sales
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Outlet> Outlets { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<SalesPoint> SalesPoints { get; set; }
        public DbSet<SalesPointStock> SalesPointStocks { get; set; }
        public DbSet<SalesPointNodeMapping> SalesPointNodeMapping { get; set; }
        public DbSet<Hierarchy> Hierarchy { get; set; }
        public DbSet<Channel> Channels { get; set; }

        #endregion

        #region Users
        public DbSet<CMUser> CMUsers { get; set; }
        public DbSet<CmsUserSalesPointMapping> CmsUserSalesPointMappings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }
        public DbSet<Delegation> Delegations { get; set; }
        public DbSet<UserTerritoryMapping> UserTerritoryMapping { get; set; }

        #endregion

        #region WorkFlows
        public DbSet<WorkFlow> WorkFlows { get; set; }
        public DbSet<WorkFlowConfiguration> WorkFlowConfigurations { get; set; }
        public DbSet<WorkflowLog> WorkflowLogs { get; set; }
        public DbSet<WorkflowLogHistory> WorkflowLogHistories { get; set; }
        public DbSet<WorkFlowType> WorkFlowTypes { get; set; }

        #endregion

        #region Campaign
        public DbSet<FMAplication.Domain.Campaign.Campaign> Campaigns { get; set; }
        public DbSet<CampaignHistory> CampaignHistories { get; set; }
        #endregion

        #region Campaign
        public DbSet<FMAplication.Domain.Brand.Brand> Brands { get; set; }
        public DbSet<SubBrand> SubBrands { get; set; }
        #endregion


        #region WareHouse
        public DbSet<FMAplication.Domain.WareHouse.WareHouse> WareHouses { get; set; }
        public DbSet<FMAplication.Domain.WareHouse.WareHouseStock> WareHouseStocks { get; set; }
        #endregion


        #region inventory

        public DbSet<Transaction.Transaction> Transactions { get; set; }
        public DbSet<Transaction.WareHouseTransfer> WareHouseTransfers { get; set; }
        public DbSet<Transaction.StockAdjustmentItems> StockAdjustmentItems { get; set; }
        public DbSet<Transaction.SalesPointAdjustmentItem> SalesPointAdjustmentItems { get; set; }
        public DbSet<Transaction.StockAddTransaction> StockAddTransactions { get; set; }
        public DbSet<Transaction.WDistributionTransaction> WDistributionTransactions { get; set; }
        public DbSet<Transaction.WDistributionRecieveTransaction> WDistributionRecieveTransactions { get; set; }
        public DbSet<Transaction.WareHouseTransferItem> WareHouseTransferItems { get; set; }
        public DbSet<WareHouseReceivedTransfer> WareHouseReceivedTransfers { get; set; }
        public DbSet<WareHouseReceivedTransferItem> WareHouseReceivedTransferItems { get; set; }
        public DbSet<SalesPointReceivedTransfer> SalesPointReceivedTransfers { get; set; }
        public DbSet<SalesPointTransfer> SalesPointTransfers { get; set; }
        public DbSet<SalesPointTransferItem> SalesPointTransferItems { get; set; }
        public DbSet<SalesPointReceivedTransferItem> SalesPointReceivedTransferItems { get; set; }

        #endregion

        #region Task
        public DbSet<PosmTaskAssign> PosmTaskAssigns { get; set; }

        public DbSet<DailyTask> DailyTasks { get; set; }
        public DbSet<DailyPosmTask> DailyPosmTasks { get; set; }
        public DbSet<DailyPosmTaskItems> DailyPosmTaskItemses { get; set; }


        public DbSet<DailySurveyTask> DailySurveyTasks { get; set; }
        public DbSet<DailySurveyTaskAnswer> DailySurveyTaskAnswers { get; set; }

        public DbSet<DailyConsumerSurveyTask> DailyConsumerSurveyTasks { get; set; }
        public DbSet<DailyConsumerSurveyTaskAnswer> DailyConsumerSurveyTaskAnswers { get; set; }


        public DbSet<DailyAVTask> DailyAvTasks { get; set; }
        public DbSet<DailyCommunicationTask> DailyCommunicationTasks { get; set; }
        public DbSet<DailyInformationTask> DailyInformationTasks { get; set; }


        public DbSet<DailyAuditTask> DailyAuditTasks { get; set; }
        public DbSet<DailyPosmAuditTask> DailyPosmAuditTasks { get; set; }
        public DbSet<DailyProductsAuditTask> DailyProductsAuditTasks { get; set; }  


        #endregion
        
        #region Survey
        public DbSet<Domain.Surveys.Survey> Surveys { get; set; }
        #endregion

        #region AVCommunication

        public DbSet<AvCommunication> AvCommunications { get; set; }
        public DbSet<AvSetup> AvSetups { get; set; }
        public DbSet<CommunicationSetup> CommunicationSetups { get; set; }

        #endregion

        #region SyncInformations
        public DbSet<SyncInformation> SyncInformations { get; set; }
        #endregion

        #region Audit
        public DbSet<AuditSetup> AuditSetups { get; set; }
        public DbSet<AuditPOSMProduct> AuditPOSMProducts { get; set; }
        public DbSet<AuditProduct> AuditProducts { get; set; }

        #endregion

        #region Reasons
        public DbSet<Reason> Reasons { get; set; }
        public DbSet<ReasonType> ReasonTypes { get; set; }
        public DbSet<ReasonReasonTypeMapping> ReasonReasonTypeMappings { get; set; }

        #endregion

        #region Guideline

        public DbSet<GuidelineSetup> GuidelineSetups { get; set; }
        #endregion

        #region transaction Workflow

        public DbSet<TransactionWorkflow> TransactionWorkflows { get; set; }
        public DbSet<TransactionNotification> TransactionNotifications { get; set; }

        #endregion

        #region MinimumExecutionLimit
        public DbSet<MinimumExecutionLimit> MinimumExecutionLimits { get; set; }

        #endregion

        #region SPWisePOSMLedger

        public DbSet<SPWisePOSMLedger> SpWisePosmLedgers { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.BuildUniqueKey();

        }



    }
}
