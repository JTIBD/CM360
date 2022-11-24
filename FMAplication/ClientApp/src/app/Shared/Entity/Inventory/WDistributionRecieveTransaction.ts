import { POSMProduct } from "../Daily-Activity";

export class WDistributionRecieveTransaction{
    id: number;
    transactionId: number;
    posmProductId: number;
    posmProduct:POSMProduct;
    quantity:number;
    recievedQuantity: number;
}