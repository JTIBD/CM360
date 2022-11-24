using System;
using System.Collections.Generic;

namespace FMAplication.RequestModels.Reports
{
    public class ExportCWDistributionReportToExcelModel
    {
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public List<int> SalesPointIds { get; set; }
    }
}