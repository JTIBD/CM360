import { Question } from "..";

export class DailySurveyTaskAnswer{
    id: number;
    dailySurveyTaskId: number;
    question: Question;
    questionId: number;
    answer: string;
}