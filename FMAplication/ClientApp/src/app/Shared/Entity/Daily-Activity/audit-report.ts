import { DailyCMActivity } from '.';

export class AuditReport {
    public  id : number; 
    public  dailyAuditId : number;      
    public  dailyCMActivityId : number;
    public  productId : number;
    public  posmProductId : number;
    public  productName : string;
    public  amount : number;
    public  actionType : number;
    
    public  uploadedImageUrl1 : string;
   
    public  uploadedImageUrl2 : string;
    public  status : number; 
    public  productImageUrl : string;

    public dailyCMActivity: DailyCMActivity;

    //display

    public displayStatus: string;
    public displayActionType: string;

    public displayDate : string;
    public fmUserName : string;
    public cmUserName : string;
    public outletName : string;
    public salesPointName : string;
}
