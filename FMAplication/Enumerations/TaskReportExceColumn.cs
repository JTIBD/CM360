using System.ComponentModel;

namespace FMAplication.Enumerations
{
    public enum TaskReportCommonColumn
    {

    }
    public enum AuditReportExceColumn
    {
        None,
        
        [Description("Date")]
        Date,
        
        [Description("CM User")]
        CM_User,

        [Description("Sales point")]
        SalesPoint,

        [Description("Route")]
        Route,

        [Description("Outlet")]
        Outlet,

        [Description("Outlet Code")]
        OutletCode,

        [Description("Product Name")]
        ProductName,
        
        [Description("Amount")]
        Amount,
        
        [Description("Action Type")]
        ActionType,
        
        [Description("Status")]
        Status,
        
        [Description("Reason")]
        Reason
    }

    public enum SurveyReportExceColumn
    {
        None,

        [Description("Date")]
        Date,
        
        [Description("CM User")]
        CM_User,

        [Description("Sales point")]
        SalesPoint,


        [Description("Route")]
        Route,

        [Description("Outlet")]
        Outlet,

        [Description("Outlet Code")]
        OutletCode,

        [Description("Survey Name")]
        SurveyName,
        
        [Description("Question")]
        Question,
        
        [Description("Answer")]
        Answer,
        
        [Description("Status")]
        Status,
        
        [Description("Reason")]
        Reason
    }

    public enum ConsumerSurveyReportExceColumn
    {
        None,

        [Description("Date")]
        Date,

        [Description("CM User")]
        CM_User,

        [Description("Sales point")]
        SalesPoint,

        [Description("Survey Name")]
        SurveyName,

        [Description("Question")]
        Question,

        [Description("Answer")]
        Answer,

    }

    public enum AvReportExceColumn
    {
        None,

        [Description("Date")]
        Date,

        [Description("CM User")]
        CM_User,

        [Description("Sales point")]
        SalesPoint,
        
        [Description("Route")]
        Route,

        [Description("Outlet")]
        Outlet,

        [Description("Outlet Code")]
        OutletCode,

        [Description("AV Name")]
        AVName,

        [Description("Status")]
        Status,

        [Description("Reason")]
        Reason

    }

    public enum CommunicationReportExceColumn
    {
        None,

        [Description("Date")]
        Date,

        [Description("CM User")]
        CM_User,

        [Description("Sales Point")]
        SalesPoint,

        [Description("Route")]
        Route,

        [Description("Outlet")]
        Outlet,

        [Description("Outlet Code")]
        OutletCode,

        [Description("Communication Name")]
        CommunicationName,

        [Description("Status")]
        Status,

        [Description("Reason")]
        Reason

    }

    public enum InformationReportExceColumn
    {
        None,

        [Description("Date")]
        Date,

        [Description("CM User")]
        CM_User,

        [Description("Sales Point")]
        SalesPoint,

        [Description("Route")]
        Route,

        [Description("Outlet")]
        Outlet,

        [Description("Outlet Code")]
        OutletCode,

        [Description("Insight")]
        Insight,

        [Description("Request")]
        Request,
        
        [Description("Status")]
        Status,

        [Description("Reason")]
        Reason

    }

    public enum POSMTaskReportExceColumn
    {
        None,

        [Description("Date")]
        Date,

        [Description("CM User")]
        CM_User,

        [Description("Sales Point")]
        SalesPoint,

        [Description("Route")]
        Route,

        [Description("Outlet")]
        Outlet,

        [Description("Outlet Code")]
        OutletCode,

        [Description("Product Name")]
        ProductName,

        [Description("Amount")]
        Amount,

        [Description("Action Type")]
        ActionType,

        [Description("Status")]
        Status,

        [Description("Reason")]
        Reason

    }

    public enum CWStockUpdateColumn
    {
        None,

        [Description("Date")]
        Date,

        [Description("POSM Code")]
        POSMCode,

        [Description("POSM Name")]
        POSMName,

        [Description("Quantity")]
        Quantity,

        [Description("Supplier")]
        Supplier,

        [Description("WareHouse Name")]
        WareHouseName
    }
    
    public enum CWDistributionReportExceColumn
    {
        None,

        [Description("Transaction Date")]
        TransactionDate,

        [Description("Received Date")]
        ReceivedDate,

        [Description("CW Name")]
        CWName,

        [Description("Region")]
        Region,

        [Description("Area")]
        Area,

        [Description("Teritory")]
        Teritory,

        [Description("Salespoint")]
        Salespoint,

        [Description("POSM Item")]
        POSMItem,

        [Description("Intransit")]
        Intransit,

        [Description("Received Quantity")]
        ReceivedQuantity,

        [Description("Distribution")]
        Distribution,

        [Description("Status")]
        Status

    }

    public enum CWStockReportExceColumn
    {
        None,

        [Description("Warehouse")]
        Warehouse,

        [Description("POSM Code")]
        POSMCode,

        [Description("POSM Name")]
        POSMName,

        [Description("Quantity")]
        Quantity,
    }

    public enum SPStockReportExceColumn
    {
        None,

        [Description("Salespoint")]
        Salespoint,

        [Description("POSM Code")]
        POSMCode,

        [Description("POSM Name")]
        POSMName,

        [Description("Quantity")]
        Quantity,
        [Description("Available Quantity")]
        AvailableQuantity,
    }

    public enum SPWisePosmLedgerReportExceColumn
    {
        None,

        [Description("Date")]
        Date,

        [Description("Region")]
        Region,

        [Description("Area")]
        Area,

        [Description("Teritory")]
        Teritory,

        [Description("Salespoint")]
        Salespoint,

        [Description("POSM Item")]
        POSMItem,

        [Description("Opening Stock")]
        OpeningStock,

        [Description("Received Stock")]
        ReceivedStock,

        [Description("Executed Stock")]
        ExecutedStock,

        [Description("Closing Stock")]
        ClosingStock
    }

    public enum DCMAReportsInDetailsExceColumn
    {
        None,

        [Description("Total execution")]
        TotalExecution,

        [Description("Area")]
        Area,

        [Description("CMR Name")]
        CmrName,

        [Description("CMR Code")]
        CmrCode,

        [Description("CMR Count")]
        CmrCount,

        [Description("DailyCMActivityId")]
        DailyCMActivityId,


        [Description("Date")]
        Date,

        [Description("Outlet Code")]
        OutletCode,

        [Description("Outlet Name")]
        OutletName,

        [Description("Outlet Type")]
        OutletType,

        [Description("Region")]
        Region,

        [Description("Route")]
        Route,

        [Description("Channel")]
        Channel,

        [Description("Sales Point")]
        SalesPoint,

        [Description("Territorry")]
        Territorry,

        [Description("TCM")]
        Tcm,

        [Description("PCM")]
        Pcm,

        [Description("Total Duration")]
        TotalDuration,

        [Description("Start Time")]
        StartTime,

        [Description("End Time")]
        EndTime,
    }
}