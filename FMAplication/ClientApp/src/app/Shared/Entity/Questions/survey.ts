import { Status } from '../../Enums/status';
import { SalesPoint } from '../Sales';

export class SurveyQuestionSet {
    public id: number;
    public name: string;
    public status: number;
    public questionsId: number[];
    public statusText: string;
    public isEditable:boolean;
    public isDeletable:boolean;

    // for client side
    isSelected: boolean;

    constructor() {
        this.id = 0;
        this.name = "";
        this.status = Status.Active;
        this.questionsId = [];
        this.isEditable = true;
    }
}
export class Survey {
    id: number;
    code: string;
    surveyQuestionSetId: number;
    surveyQuestionSet?: SurveyQuestionSet;
    salesPointId: number;
    salesPoint?: SalesPoint;
    userType: number;
    isConsumerSurvey:boolean;
    fromDate: string;
    fromDateStr: string;
    toDate: string;
    toDateStr: string;
    status: number;
}
