import { Outlet } from "..";
import { Reason } from "../ExecutionReason/ExecutionReason";
import { DailyBaseTask } from "./dailyBaseTask";

export class DailyInformationTask extends DailyBaseTask{
    insightImage: string;
    insightDescription: string;
    requestImage: string;
    requestDescription: string;
}