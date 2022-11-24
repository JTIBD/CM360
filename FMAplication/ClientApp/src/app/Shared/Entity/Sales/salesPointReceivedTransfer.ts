import { SalesPoint } from ".";
import { SalesPointTransfer } from "./salesPointTransfer";
import { SalesPointReceivedTransferItem } from "./salesPointReceivedTransferItem";

export class SalesPointReceivedTransfer{
    id: number;
    transactionNumber: string;
    remarks: string;
    isConfirmed: boolean;
    transactionStatus: number;
    transactionDate: string;
    transactionDateStr: string;
    fromSalesPointId: number;
    fromSalesPoint: SalesPoint;
    toSalesPointId: number;
    toSalesPoint: SalesPoint;
    sourceTransferId: number;
    items: SalesPointReceivedTransferItem[];
    sourceTransfer: SalesPointTransfer;
}