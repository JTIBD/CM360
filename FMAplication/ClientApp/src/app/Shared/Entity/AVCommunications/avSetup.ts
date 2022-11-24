import { SalesPoint } from "..";
import { AvCommunication } from "./avCommunication";
import { IPTableSetting, colDef, IPtableMasterData } from 'src/app/Shared/Modules/p-table';

export class AvSetup{
    id: number;
    code: string;
    avId: number;
    av?: AvCommunication;
    salesPointId: number;
    salesPoint?: SalesPoint;
    userType: number;
    fromDate: string;
    fromDateStr: string;
    toDate: string;
    toDateStr: string;
    status: number;
}

export class AvSetupTableData implements IPtableMasterData {
    id:number;
    salesPointCode:string;
    salesPointName:string;
    startDate:string;
    endDate:string;
    avName:string;
    userType:string;
    status: string;    
    disableCheck: boolean | false;
    disableEdit: boolean | false;
    disableDelete: boolean | false;
    disableView: boolean | true;
}