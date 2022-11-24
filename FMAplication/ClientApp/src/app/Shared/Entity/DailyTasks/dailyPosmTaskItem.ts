import { PosmProduct } from "..";
import { PosmWorkType } from "../../Enums";

export class DailyPosmTaskItem{
    id: number;
    dailyPosmTaskId: number;
    posmProduct: PosmProduct;
    posmProductId: number;
    quantity: number;
    executionType: PosmWorkType;
    image: string;
}