import { Status } from "../../Enums/status";
import { ReasonReasonTypeMapping } from "./ReasonReasonTypeMapping";

export class Reason {
    public id: number;
    public name: string;
    public reasonInEnglish: string;
    public reasonInBangla: string;
    public status: number;
    reasonReasonTypeMappings:ReasonReasonTypeMapping[]=[];
    
    constructor() {

    }
}