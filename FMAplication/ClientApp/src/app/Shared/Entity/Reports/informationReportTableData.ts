import { BaseReportTableData } from "./baseReportTableData";

export class InformationReportTableData extends BaseReportTableData{  
    informationName:string;  
    request:string; 
    requestImage:string; 
    
    insight:string; 
    insightImage:string;
    viewRequestImageBtnText:string = ""; 
    viewInsightImageBtnText:string = ""; 
}