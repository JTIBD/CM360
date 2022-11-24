import { BaseReportTableData } from "../Reports/baseReportTableData";

export class DailyPOSMReportTableData extends BaseReportTableData{
    productName:string;
    productImage : string; 
    newImage:string;
    existingImage:string;
    amount:number;
    displayActionType:string;
    reason:string;
    viewButtonTextForNewImage:string="";
    viewButtonTextForExistingImage : string = "";
    viewButtonTextForProductImage : string = "";
}