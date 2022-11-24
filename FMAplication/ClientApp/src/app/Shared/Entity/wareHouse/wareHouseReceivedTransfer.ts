import { WareHouse } from "../Inventory";
import { WareHouseReceivedTransferItem } from "./wareHouseReceivedTransferItem";

export class WareHouseReceivedTransfer{
    id: number;
    transactionNumber: string;
    remarks: string;
    isConfirmed: boolean;
    transactionStatus: number;
    transactionDate: string;
    transactionDateStr: string;
    fromWarehouseId: number;
    fromWarehouse: WareHouse;
    toWarehouseId: number;
    toWarehouse: WareHouse;
    sourceTransferId:number;
    items: WareHouseReceivedTransferItem[];
}