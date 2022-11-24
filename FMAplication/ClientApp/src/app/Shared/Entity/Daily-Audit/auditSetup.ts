import { SalesPoint } from "..";
import { TaskAssignedUserType } from "../../Enums";
import { AuditPOSMProduct } from "./auditPOSMProduct";
import { AuditProduct } from "./auditProduct";

export class AuditSetup{
    id: number;
    code: string;
    salesPointId: number;
    salesPoint: SalesPoint;
    userType: TaskAssignedUserType;
    fromDate: string;
    fromDateStr: string;
    toDate: string;
    toDateStr: string;
    auditProducts: AuditProduct[];
    auditPOSMProducts: AuditPOSMProduct[];
    status: Number;
}