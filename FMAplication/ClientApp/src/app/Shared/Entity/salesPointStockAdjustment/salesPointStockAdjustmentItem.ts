import { PosmProduct } from "..";

export class SalesPointStockAdjustmentItem{
    id: number;
    transactionId: number;
    posmProductId: number;
    posmProduct: PosmProduct;
    systemQuantity: number;
    adjustedQuantity: number;
}