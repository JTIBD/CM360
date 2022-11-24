import { IPtableMasterData } from "../../Modules/p-table";
import { BaseReportTableData } from "./baseReportTableData";

export class SurveyReportTableData extends BaseReportTableData implements IPtableMasterData{    
    surveyName:string;
    questionName:string;
    answer:string;
    questionType:string;
    btnIcon="fa fa-info";
    btnTitle="";
}