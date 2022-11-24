import { PosmProduct } from "..";
import { ActionType } from "../../Enums/actionType";

export class DailyPosmAuditTask{
    id: number;
    createdBy: number;
    createdTime: Date;
    createdTimeStr: string;
    modifiedBy: number;
    modifiedTime: Date;
    modifiedTimeStr: string;
    status: number;
    posmProduct: PosmProduct;
    posmProductId: number;
    actionType: ActionType;
    result: number;
    dailyAuditTaskId: number;    
}