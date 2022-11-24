import { IPtableMasterData } from "../../Modules/p-table";

export class StockReceiveTableData implements IPtableMasterData{
    transactionId:number;
    centralWareHouseName:string;
    salesPointName:string;
    transactionNumber:string;
    chalanNumber:string;
    date:string;
    lines:number;
    quantity:number;
    isConfirmed:boolean;
    disableCheck:boolean=false;
    checkBtnTitle:string ="Confirm";
    viewBtnTitle:string = "View distribution details";
    transactionStatus:string=""
    transactionDate:string="";

}