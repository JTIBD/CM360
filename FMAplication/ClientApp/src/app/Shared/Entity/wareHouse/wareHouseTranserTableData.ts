export class WareHouseTranserTableData{
    id: number;
    fromWareHouseName: string;
    fromWareHouseCode:string;
    toWareHouseName: string;
    toWareHouseCode:string;
    date: string;
    line:number;
    totalQuantity: number;
    status:string;
    transactionNumber:string;
    isConfirmed:boolean=false;
    disableCheck=false;
    transferType:string="";
}