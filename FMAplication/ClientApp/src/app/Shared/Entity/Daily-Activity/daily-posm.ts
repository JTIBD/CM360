export class DailyPOSM{
    public id:number;
    public dailyCMActivityId:number;
    public isPOSMInstallation:boolean;
    public isPOSMRemoval:boolean;
    public isPOSMRepair:boolean;
    public posmInstallationIncompleteReason:string;
    public posmInstallationStatus:string;
    public posmRemovalIncompleteReason:string;
    public posmRemovalStatus:string;
    public posmRepairIncompleteReason:string;
    public posmRepairStatus:string;
    public posmRepairProducts:POSMProduct[]=[];
    public posmInstallationProducts:POSMProduct[]=[];
    public posmRemovalProducts:POSMProduct[]=[];
  }

  export class POSMProduct{
    // public :number;
    public  actionType: any;
    public  productType: number;
    public	amount: number;
    public	dailyCMActivityId: number;
    public	productId: number;
    public	uploadedImageUrl1: string;
    public	uploadedImageUrl2: string;
    name:string;
  }
