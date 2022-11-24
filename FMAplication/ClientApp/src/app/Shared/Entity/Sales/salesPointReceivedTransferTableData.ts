export class SalesPointReceivedTransferTableData{
    id: number;
    fromSalesPoint: string;
    toSalesPoint: string;
    date: string;
    line:number;
    totalQuantity: number;
    status:string;
    transactionNumber:string;
    isConfirmed:boolean=false;
    disableCheck=false;
    transferType:string="";
}