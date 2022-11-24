import { MapObject } from './mapObject';

export enum ActionType {
    DistributionCheckProduct = 0,
    FacingCountProduct = 1,
    PlanogramCheckProduct = 2,
    PriceAuditProduct = 3
}

export class ActionTypeLabel{

    public static ActionType :  MapObject[] = [
    { id : 0, label : "Distribution Check" },
    { id : 1, label : "Facing Count" },
    { id : 2, label : "Planogram Check" },
    { id : 3, label : "Price Audit" }
    
    ];


}


