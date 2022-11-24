import { PosmProduct } from "..";

export class WareHouseReceivedTransferItem{
    id: number;
    transactionId: number;
    posmProductId: number;
    posmProduct: PosmProduct;
    quantity: number;
    receivedQuantity:number;
}