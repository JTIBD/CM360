import { PosmProduct } from "..";

export class SalesPointReceivedTransferItem{
    id: number;
    transferId: number;
    posmProductId: number;
    posmProduct: PosmProduct;
    quantity: number;
    receivedQuantity: number;
}