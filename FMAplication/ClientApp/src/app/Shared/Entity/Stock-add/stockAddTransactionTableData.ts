import { IPtableMasterData } from "../../Modules/p-table";

export class StockAddTransactionTableData implements IPtableMasterData{
    transactionId:number;
    centralWareHouseName:string;
    transactionNumber:string;
    date:string;
    lines:number;
    quantity:number;
    isConfirmed:boolean;
    disableCheck:boolean=false;
    checkBtnTitle:string ="Confirm";
    viewBtnTitle:string = "View details";
    transactionStatus:string="";
    remarks:string="";
    disableView:boolean=false;
    disableEdit:boolean=false;
}