import { Outlet } from "..";
import { Reason } from "../ExecutionReason/ExecutionReason";

export class DailyBaseTask{
    id: number;
    dailyTaskId: number;
    outletId: number;
    outlet: Outlet;
    isCompleted: boolean;
    isOutletOpen:boolean;
    reason: Reason;
    reasonId: number;
}