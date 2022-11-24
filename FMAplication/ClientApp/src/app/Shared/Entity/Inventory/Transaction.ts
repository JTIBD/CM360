import { SalesPoint, UserInfo, WDistributionTransaction } from '..';
import { TransactionStatus } from '../../Enums/TransactionStatus';
import { TransactionType } from '../../Enums/TransactionType';
import { TransactionNotification } from '../Notification/TransactionNotification';
import { SalesPointStockAdjustmentItem } from '../salesPointStockAdjustment';
import { TransactionWorkflow } from '../TransactionWorkflow/TransactionWorkflow';
import { StockAddTransaction } from './StockAddTransaction';
import { WareHouse } from './WareHouse';
import { WDistributionRecieveTransaction } from './wDistributionRecieveTransaction';

export class Transaction {
    public id: number;
    createdBy:number;
    createdByUser:UserInfo;
    referenceTransactionId:number;
    referenceTransaction:Transaction;
    public transactionSerial: number;
    public remarks: string; 
    public isConfirmed: boolean=false; 
    public transactionStatus: TransactionStatus; 
    public transactionType: TransactionType;
    public transactionDate: string=new Date().toISOString();
    transactionDateStr:string = new Date().toISOString();;
    public transactionNumber: string=""; 
    public chalanNumber: string; 
    public wareHouseModel: WareHouse;
    public warehouseId: number; 
    public salesPoint: SalesPoint;
    public salesPointId: number;
    transactionWorkflow:TransactionWorkflow;
    transactionNotification:TransactionNotification
    public stockAddTransactions: StockAddTransaction[]=[];
    public wDistributionTransactions: WDistributionTransaction[]=[];
    wDistributionRecieveTransactions:WDistributionRecieveTransaction[]=[];
    salesPointAdjustmentItems:SalesPointStockAdjustmentItem[]=[];

}