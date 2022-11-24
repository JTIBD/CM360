import { Outlet } from "..";
import { AvSetup } from "../AVCommunications/avSetup";
import { Reason } from "../ExecutionReason/ExecutionReason";
import { DailyBaseTask } from "./dailyBaseTask";

export class DailyAVTask extends DailyBaseTask{
    id: number;
    avSetupId: number;
    avSetup:AvSetup;    
}