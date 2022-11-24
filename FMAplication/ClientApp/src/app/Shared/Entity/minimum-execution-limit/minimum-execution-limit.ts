import { SalesPoint } from '..';

export class MinimumExecutionLimit{
    id: number;
    code: string;
    salesPointId: number;
    salesPoint?: SalesPoint;
    targetVisitedOutlet: number;
}

export interface IGetMinimumExecutionLimitModel{
    pageSize: number;
    pageIndex: number;
    searchText: string;
    targetVisitedOutlet: string;
    salesPointId: number;
}

export class MinimumExecutionLimitTableData{
    id:number;
    salesPointCode:string;
    salesPointName:string;
    targetVisitedOutlet: number;
}