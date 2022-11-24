using fm_application.Models.DailyActivity;
using FMAplication.Models.DailyActivity;
using FMAplication.Models.Products;
using FMAplication.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Common
{
    public class DashboardModel
    {
        public DashboardModel()
        {
            
        }


        #region POSM Summary data
        public int TotalCurrentMonthPosmInstallationProductCount { get; set; }
        public int TotalCurrentMonthPosmRepairProductCount { get; set; }
        public int TotalCurrentMonthPosmRemovalProductCount { get; set; }
        public int TotalCurrentMonthPosmRemovalAndReInstallationCount { get; set; }
        public int TotalCurrentMonthAuditReportProductCount { get; set; }
        public int TotalCurrentMonthSurveyReportProductCount { get; set; }
        public int TotalCurrentMonthConsumerSurveyReportProductCount { get; set; }
        #endregion

        #region Tiles data last month
        public int TotalLastMonthPosmInstallationProductCount { get; set; }
        public int TotalLastMonthPosmRepairProductCount { get; set; }
        public int TotalLastMonthPosmRemovalProductCount { get; set; }
        public int TotalLastMonthPosmRemovalAndReInstallationCount { get; set; }
        public int TotalLastMonthAuditReportProductCount { get; set; }
        public int TotalLastMonthSurveyReportProductCount { get; set; }
        public int TotalLastMonthConsumerSurveyReportProductCount { get; set; }
        #endregion

       

    }
}
