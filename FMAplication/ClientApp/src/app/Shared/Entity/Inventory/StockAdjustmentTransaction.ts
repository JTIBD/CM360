import { TransactionNotification } from 'src/app/Shared/Entity/Notification/TransactionNotification';
import { TransactionWorkflow } from './../TransactionWorkflow/TransactionWorkflow';
import { HttpParams } from "@angular/common/http";
import { WareHouse } from ".";
import { PosmProduct, SalesPoint } from "..";
import { TransactionStatus } from "../../Enums/TransactionStatus";
import { TransactionType } from "../../Enums/TransactionType";
import { IPtableMasterData } from "../../Modules/p-table";
import { Transaction } from "./Transaction";



export class StockAdjustmentTableData implements IPtableMasterData{
  id:number;
  transactionNumber:string;
  transactionDate:string;
  lines:number; 
  totalDecrease:number; 
  totalIncrease:number; 
  isConfirmed:boolean;
  status : string;
  remarks : string;
  
  disableCheck:boolean=false;
  disableView:boolean = false;
  disableEdit:boolean = false;
  checkBtnTitle:string ="Confirm";
  viewBtnTitle:string = "View Transaction details"


}

export class IStockAdjustmentTransaction {
    id: number;
    transactionNumber: string;
    remarks: string;
    isConfirmed: boolean
    transactionDate: string;
    transactionId: number;
    stockAdjustmentItems: IStockStockAdjustmentItem[];
    lines:number; 
    totalDecrease:number; 
    totalIncrease:number; 
    wareHouseModel: WareHouse;
    warehouseId: number; 
    transactionWorkflow:TransactionWorkflow;
    transactionNotification:TransactionNotification
    transactionStatus: TransactionStatus; 
    transactionType: TransactionType;
  }
  
  export class IStockStockAdjustmentItem {
    id: number;
  
    transactionId: number;
    transaction?: any;
    posmProductId: number;
    posmProduct?: PosmProduct;
    systemQuantity: number;
    adjustedQuantity: number;
  }

  export class AdjustmentTransactionParams { 
    /**
     *
     */
    constructor() {
      this.pageIndex = 1;
      this.search = "";
      this.transactionStatus = 0;
      this.pageSize = 10;
      let date = new Date();
      this.fromDate = new Date(date.getFullYear(), date.getMonth() , 1).toISOString();
      this.toDate=  new Date().toISOString();
      
    }
    pageSize:number; 
    pageIndex:number;
    transactionStatus: number;
    search:string;
    sort : string;
    fromDate:string; 
    toDate:string;
    
  }