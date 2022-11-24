import { SalesPoint } from ".";
import { PosmProduct } from "..";

export class SalesPointTransferItem {
    id: number;
    transferId: number;
    posmProductId: number;
    posmProduct: PosmProduct;
    quantity: number;
}