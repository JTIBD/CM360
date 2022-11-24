import { Product } from "..";
import { ActionType } from "../../Enums/actionType";

export class AuditProduct {
    id: number;
    auditSetupId: number;
    productId: number;
    product: Product;
    actionType: ActionType;
}