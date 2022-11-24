import { IPtableMasterData } from "../../Modules/p-table";
import { Question } from "./question";

export class QuestionBankTableData extends Question implements IPtableMasterData{
    disableDelete=false;
    disableEdit = false;
}