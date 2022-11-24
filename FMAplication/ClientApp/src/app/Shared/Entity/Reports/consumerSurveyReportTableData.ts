import { IPtableMasterData } from "../../Modules/p-table";
import { BaseReportTableData } from "./baseReportTableData";

export class ConsumerSurveyReportTableData extends BaseReportTableData implements IPtableMasterData{    
    surveyName:string;
    questionName:string;
    answer:string;
    btnIcon="fa fa-info";
    btnTitle="";    
}