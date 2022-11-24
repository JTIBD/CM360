import { PosmProduct } from '..';
import { Transaction } from './Transaction';

export class StockAddTransaction {
    public id: number;
    public transactionId: number;  
    public transactionModel:Transaction;
    public posmProductId: number;  
    public posmProductModel: PosmProduct;
    public quantity: number;  
    public supplier: string;
}