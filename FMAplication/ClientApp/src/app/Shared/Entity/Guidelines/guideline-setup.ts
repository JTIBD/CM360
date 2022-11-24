import { SalesPoint } from '..';
import { IPtableMasterData } from '../../Modules/p-table';

export class GuidelineSetup{
    id: number;
    code: string;
    guidelineText: string;
    salesPointId: number;
    salesPoint?: SalesPoint;
    userType: number;
    fromDate: string;
    fromDateStr: string;
    toDate: string;
    toDateStr: string;
    status: number;
}

export class GuidelineSetupTableData implements IPtableMasterData{
    id:number;
    salesPointCode:string;
    salesPointName:string;
    startDate:string;
    endDate:string;
    guidelineName: string;
    userType:string;  
    status:string;  
    disableCheck: boolean | false;
    disableView: boolean | false;
    disableDelete: boolean | false;
    disableEdit: boolean | false;
}

export interface IGetGuidelineSetupQueryModel{
    pageSize: number;
    pageIndex: number;
    search: string;
    fromDateTime: string;
    toDateTime: string;
    salesPointId: number;
}