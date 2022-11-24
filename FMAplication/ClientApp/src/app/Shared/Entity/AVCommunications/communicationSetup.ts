import { SalesPoint } from "..";
import { IPtableMasterData } from "../../Modules/p-table";
import { AvCommunication } from "./avCommunication";

export class CommunicationSetup{
    id: number;
    code: string;
    avCommunicationId: number;
    avCommunication?: AvCommunication;
    salesPointId: number;
    salesPoint?: SalesPoint;
    userType: number;
    fromDate: string;
    fromDateStr: string;
    toDate: string;
    toDateStr: string;
    status: number;
}

export class CommunicationSetupTableData implements IPtableMasterData {
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