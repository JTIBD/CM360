import { DailyAuditTask } from ".";
import { CmUser, SalesPoint } from "../PosmAssign/posmAssign";
import { DailyAVTask } from "./dailyAVTask";
import { DailyCommunicationTask } from "./dailyCommunicationTask";
import { DailyConsumerSurveyTask } from "./dailyConsumerSurveyTask";
import { DailyInformationTask } from "./dailyInformationTask";
import { DailyPosmTask } from "./dailyPosmTask";
import { DailySurveyTask } from "./dailySurveyTask ";

export class DailyTask{
    id: number;
    cmUser: CmUser;
    cmUserId: number;
    salesPoint: SalesPoint;
    salesPointId: number;
    dateTimeStr: string;
    isSubmitted: boolean;
    dailyPosmTasks: DailyPosmTask[];
    dailyAuditTasks: DailyAuditTask[];
    dailySurveyTasks: DailySurveyTask[];
    dailyConsumerSurveyTasks: DailyConsumerSurveyTask[];
    dailyAVTasks: DailyAVTask[];
    dailyCommunicationTasks: DailyCommunicationTask[];
    dailyInformationTasks: DailyInformationTask[];
}