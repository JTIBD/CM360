import { SalesPoint } from '.';
import { PosmProduct } from '..';
import {Selectable} from '../Common'
export class SalespointStock extends Selectable{
    id: number;
    salesPointId: number;
    salesPoint: SalesPoint;
    posmProductId: number;
    posmProduct: PosmProduct;
    quantity: number;
    availableQuantity:number;
}

export class SalesPointStockTableData{
    id: number;
    salesPointName: string;
    posmProductName: string;
    posmCode:string;
    quantity: number;
    availableQuantity:number;
}

       