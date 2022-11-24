import { QuestionOption } from './questionOption';
import { Status } from '../../Enums/status';

export class Question {
    public id: number;
    public questionTitle: string;
    public questionType: string;
    public questionOptions: QuestionOption[];
    public status: number;
    public isEditable:boolean;

    //for survey generation
    public addToSurveyBtn: string;

    constructor() {
        this.id = 0;
        this.questionTitle = '';
        this.questionType = '';
        this.status = Status.Active;

        //for survey generation
        this.addToSurveyBtn = "";
        this.questionOptions = [];
        this.isEditable = true;
    }
}