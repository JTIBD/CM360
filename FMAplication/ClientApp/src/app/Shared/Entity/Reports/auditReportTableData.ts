import { BaseReportTableData } from "./baseReportTableData";

export class AuditReportTableData extends BaseReportTableData{        
    productName:string;
    amount:number|string;
    displayActionType:string;    
}