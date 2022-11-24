import { IPosmProductStock, WareHouse } from "../Inventory";

export class WareHouseStock{
    id: number;
    wareHouseId: number;
    wareHouse: WareHouse;
    posmProductId: number;
    posmProduct: IPosmProductStock;
    quantity: number;
    availableQuantity:number;
}