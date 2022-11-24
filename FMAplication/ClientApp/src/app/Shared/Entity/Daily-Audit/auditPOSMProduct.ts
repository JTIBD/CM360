import { PosmProduct } from "..";
import { ActionType } from "../../Enums/actionType";

export class AuditPOSMProduct {
    id: number;
    auditSetupId: number;
    posmProductId: number;
    posmProduct: PosmProduct;
    actionType: ActionType;
}
