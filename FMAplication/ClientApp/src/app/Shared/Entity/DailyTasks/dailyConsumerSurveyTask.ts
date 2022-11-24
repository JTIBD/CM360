import { SurveyQuestionSet } from "..";
import { Reason } from "../ExecutionReason/ExecutionReason";
import { DailyConsumerSurveyTaskAnswer } from "./dailyConsumerSurveyTaskAnswer";

export class DailyConsumerSurveyTask{
    id: number;
    createdBy: number;
    createdTime: Date;
    createdTimeStr: string;
    modifiedBy: number;
    modifiedTime: Date;
    modifiedTimeStr: string;
    status: number;
    dailyTaskId: number;
    surveyQuestionSet: SurveyQuestionSet;
    surveyQuestionSetId: number;
    isCompleted: boolean;
    reason: Reason;
    reasonId: number;
    dailyConsumerSurveyTaskAnswers: DailyConsumerSurveyTaskAnswer[];    
}