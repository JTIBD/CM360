import { Question } from '..';
import { SurveyQuestionSet } from '../Questions';
import { DailyCMActivity } from './daily-cm-activity';

export class SurveyReport {

    public id : number;
    public dailyCMActivityId : number;
    public questionId : number;
    public answer : string;
    public surveyId : number;
    public isConsumerSurvey : boolean;

    public status: number;

    public question: Question;
    public survey : SurveyQuestionSet;
    public dailyCMActivity: DailyCMActivity;

    // display

    public displayStatus: string;
    public displayIsConsumerSurvey: string;
    public surveyName : string;
    public questionName : string;

    public displayDate : string;
    public fmUserName : string;
    public cmUserName : string;
    public outletName : string;
    public salesPointName : string;
}
