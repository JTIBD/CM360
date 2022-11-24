import { Status } from '../../Enums/status';

export class QuestionOption {
    public id: number;
    public questionId: number;

    public optionTitle: string;
    public sequence: number;
    public status: number;
    public statusText : string;
    public serialNumber : number;

    constructor() {
        this.id = 0;
        this.questionId = 0;
        this.optionTitle = '';
        this.sequence = 0;
        this.status = Status.Active;
        this.serialNumber = 0;
    }
}