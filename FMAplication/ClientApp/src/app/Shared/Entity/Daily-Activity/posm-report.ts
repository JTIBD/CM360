import { DailyCMActivity } from '.';

export class PosmReport {

    public  id : number;
        public  dailyPOSMId : number;
        public  dailyCMActivityId : number;
        public  productId : number;
        public  productName : string;
        public  amount : number;
        public  actionType : number;
        public  productType : number;
        public  productPlanogramImageUrl : string;
        
        public  uploadedImageUrl1 : string;
       
        public  uploadedImageUrl2 : string;

        public  status : number;

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
