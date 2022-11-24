import { WareHouse } from "../Inventory";
import { WareHouseTransferItem } from "./wareHouseTransferItem";

export class WareHouseTransfer {
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
    items: WareHouseTransferItem[];
}