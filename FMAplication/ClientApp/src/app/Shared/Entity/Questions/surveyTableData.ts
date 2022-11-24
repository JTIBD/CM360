
export class SurveyTableData{
    surveyId:number;
    salesPointCode:string;
    salesPointName:string;
    startDate:string;
    endDate:string;
    questionSet:string;
    userType:string;
    surveyType:string;    
    status:string;  
    disableCheck: boolean | false;
    disableView: boolean | false;
    disableDelete: boolean | false;
    disableEdit: boolean | false;
}