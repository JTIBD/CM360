export class CreatePOSM_Distribution{
    wareHouseCode:string;
    wDistributionTransactionProducts:WDistributionTransactionProduct[];
}

export class WDistributionTransactionProduct{
     posM_Name:string;
     salesPointCode:string;
     quantity:number;
}