import { PosmProduct } from "..";

export class WareHouseTransferItem{
    id: number;
    transactionId: number;
    posmProductId: number;
    posmProduct: PosmProduct;
    quantity: number;
}