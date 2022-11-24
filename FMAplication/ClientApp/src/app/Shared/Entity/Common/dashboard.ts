import { POSMProduct, DailyCMActivity } from '../Daily-Activity'
import { User } from '..';
import { DailyCmTaskReport } from '../Daily-Activity/daily-cm-task-report';

export class Dashboard {
    // public posmProductList : POSMProduct[];
    //#region POSM Summary data
    public totalCurrentMonthPosmInstallationProductCount: number;
    public totalCurrentMonthPosmRepairProductCount: number;
    public totalCurrentMonthPosmRemovalProductCount: number;
    public totalCurrentMonthPosmRemovalAndReInstallationCount: number;
    public totalCurrentMonthAuditReportProductCount: number;
    public totalCurrentMonthSurveyReportProductCount: number;
    public totalCurrentMonthConsumerSurveyReportProductCount: number;
    //#endregion

  

    //#region CM Activity graph data
   

  
    //#endregion

    //#region Tiles data
    public totalLastMonthPosmInstallationProductCount: number;
    public totalLastMonthPosmRepairProductCount: number;
    public totalLastMonthPosmRemovalProductCount: number;
    public  totalLastMonthPosmRemovalAndReInstallationCount: number;
    public totalLastMonthAuditReportProductCount : number;
    public totalLastMonthSurveyReportProductCount : number;
    public totalLastMonthConsumerSurveyReportProductCount : number;
    //#endregion

    constructor(){

        this.totalCurrentMonthPosmInstallationProductCount = 0;
        this.totalCurrentMonthPosmRepairProductCount = 0;
        this.totalCurrentMonthPosmRemovalProductCount = 0;
        this.totalCurrentMonthPosmRemovalAndReInstallationCount = 0;
        this.totalCurrentMonthAuditReportProductCount = 0;
        this.totalCurrentMonthSurveyReportProductCount = 0;
        this.totalCurrentMonthConsumerSurveyReportProductCount = 0; 

        this.totalLastMonthPosmInstallationProductCount = 0;
        this.totalLastMonthPosmRepairProductCount = 0;
        this.totalLastMonthPosmRemovalProductCount = 0;
        this.totalLastMonthPosmRemovalAndReInstallationCount = 0;
        this.totalLastMonthAuditReportProductCount = 0;
        this.totalLastMonthSurveyReportProductCount = 0;
        this.totalLastMonthConsumerSurveyReportProductCount = 0;
    }
}
