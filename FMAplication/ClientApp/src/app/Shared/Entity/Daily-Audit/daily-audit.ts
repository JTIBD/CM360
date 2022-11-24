import { PosmInstallationStatus } from '../../Enums/posm-installation-status.enum';


export class DailyAudit {

    public  id  : number;
        public  dailyCMActivityId : number;
        public  isDistributionCheck : boolean;
        public  isFacingCount : boolean;
        public  isPlanogramCheck : boolean;
        public  isPriceAudit : boolean;
        public  distributionCheckStatus : string;
        public  facingCountStatus : string;
        public  planogramCheckStatus : string;
        public  priceAuditCheckStatus : string;
        
        public  distributionCheckIncompleteReason : string;
       
        public  facingCountCheckIncompleteReason  : string;
        
        public  planogramCheckIncompleteReason : string;
       
        public  priceAuditCheckIncompleteReason : string;


        constructor(){
            this.distributionCheckStatus = Object.keys(PosmInstallationStatus).filter(k => isNaN(Number(k)))[0];
            this.facingCountStatus = Object.keys(PosmInstallationStatus).filter(k => isNaN(Number(k)))[0];
            this.planogramCheckStatus = Object.keys(PosmInstallationStatus).filter(k => isNaN(Number(k)))[0];
            this.priceAuditCheckStatus = Object.keys(PosmInstallationStatus).filter(k => isNaN(Number(k)))[0];
        }
}
