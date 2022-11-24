
  export class DailyAudit{
    public id:number;
    public dailyCMActivityId:number;


    public isDistributionCheck: boolean;
    public distributionCheckProducts:any[];
    public distributionCheckIncompleteReason:string;
    public distributionCheckStatus:number;

    public isFacingCount: boolean;
    public facingCountProducts:any[];
    public facingCountCheckIncompleteReason:string;
    public facingCountStatus:number;

    public isPlanogramCheck: boolean;
    public planogramCheckProducts: any[];
    public planogramCheckIncompleteReason: string;
    public planogramCheckStatus: number;

    public isPriceAudit: boolean;
    public priceAuditProducts: any[];
    public priceAuditCheckIncompleteReason: string;
    public priceAuditCheckStatus: string;

  }