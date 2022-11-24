namespace FMAplication.Models.Reports
{
    public class CWDistributionReportToExcelData
    {
        public string TransactionDate { get; set; }
        public string ReceivedDate { get; set; }
        public string POSMName { get; set; }
        public string WareHouseName { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public string Teritory { get; set; }
        public string SalesPoint { get; set; }
        public int Intransit { get; set; }
        public int Distribution { get; set; }
        public int ReceivedQuantity { get; set; }
        public string Status{ get; set; }
        public bool IsReceived { get; set; }

    }
}