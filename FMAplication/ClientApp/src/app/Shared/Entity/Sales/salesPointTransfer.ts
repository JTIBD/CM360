import { SalesPoint } from ".";
import { TransactionStatus } from "../../Enums/TransactionStatus";
import { TransactionNotification } from "../Notification/TransactionNotification";
import { TransactionWorkflow } from "../TransactionWorkflow/TransactionWorkflow";
import { SalesPointTransferItem } from "./salesPointTransferItem";

export class SalesPointTransfer{
    id: number;
    transactionNumber: string;
    remarks: string;
    isConfirmed: boolean;
    transactionStatus: TransactionStatus;
    transactionDate: string;
    transactionDateStr: string;
    fromSalesPointId: number;
    fromSalesPoint: SalesPoint;
    toSalesPointId: number;
    toSalesPoint: SalesPoint;
    transactionWorkflow:TransactionWorkflow;
    transactionNotification:TransactionNotification
    items: SalesPointTransferItem[]=[];
}