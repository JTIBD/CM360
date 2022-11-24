import { Outlet } from "..";
import { Reason } from "../ExecutionReason/ExecutionReason";
import { DailyBaseTask } from "./dailyBaseTask";
import { DailyPosmAuditTask } from "./dailyPosmAuditTask";
import { DailyProductsAuditTask } from "./dailyProductsAuditTask";

export class DailyAuditTask extends DailyBaseTask{
    createdBy: number;
    createdTime: Date;
    createdTimeStr: string;
    modifiedBy: number;
    modifiedTime: Date;
    modifiedTimeStr: string;
    status: number;
    auditSetupId: number;  
    dailyProductsAuditTask: DailyProductsAuditTask[];
}