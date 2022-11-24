import { IPtableMasterData } from "../../Modules/p-table";

export class SalesPointTransferTableData implements IPtableMasterData{
    id:number;
    sourceSalesPointName:string;
    destinationSalesPointName:string;
    transactionNumber:string;
    date:string;
    lines:number;
    quantity:number;
    isConfirmed:boolean;
    disableCheck:boolean=false;
    checkBtnTitle:string ="Confirm";
    viewBtnTitle:string = "View details";
    transactionStatus:string=""
}