import { Question } from "..";

export class DailyConsumerSurveyTaskAnswer{
    id: number;
    createdBy: number;
    createdTime: Date;
    createdTimeStr: string;
    modifiedBy: number;
    modifiedTime: Date;
    modifiedTimeStr: string;
    status: number;
    dailyConsumerSurveyTaskId: number;
    question: Question;
    questionId: number;
    answer: string;    
}