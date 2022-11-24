export class CreateStockAddTransaction {
    wareHouseCode: string;
    remarks: string;
    posM_Products: POSM_Product_Quantity[];
}

export class POSM_Product_Quantity {
    posmProductCode: string;
    quantity: number;
    supplier: string;
}