import { Product } from "..";
import { ActionType } from "../../Enums/actionType";
import { PosmProduct } from "../Products";

export class DailyProductsAuditTask{
    id: number;
    createdBy: number;
    createdTime: Date;
    createdTimeStr: string;
    modifiedBy: number;
    modifiedTime: Date;
    modifiedTimeStr: string;
    status: number;
    product: Product;
    productId: number;
    posmProduct:PosmProduct;
    actionType: ActionType;
    result: number;
    dailyAuditTaskId: number;    
}