import { IPtableMasterData } from "../../Modules/p-table";
import { SurveyQuestionSet } from "./survey";

export class SurveyQuestionSetTableData extends SurveyQuestionSet implements IPtableMasterData{
    disableEdit = false;
    disableDelete = false;
    disableView=false;
}