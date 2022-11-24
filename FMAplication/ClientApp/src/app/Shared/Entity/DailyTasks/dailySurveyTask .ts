import { SurveyQuestionSet } from "..";
import { DailyBaseTask } from "./dailyBaseTask";
import { DailySurveyTaskAnswer } from "./dailySurveyTaskAnswer";

export class DailySurveyTask extends DailyBaseTask{
    surveyQuestionSet: SurveyQuestionSet;
    surveyQuestionSetId: number;
    dailySurveyTaskAnswers: DailySurveyTaskAnswer[];
}