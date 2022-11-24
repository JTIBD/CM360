import { PosmProduct } from "../Products";

export interface IPosmProductStock extends PosmProduct{
    name: string;
    type: number;
    isJTIProduct: boolean;
    isDigitalSignatureEnable: boolean;
    isPlanogram: boolean;
    planogramImageUrl: string;
    code: string;
    campaignId: number;
    brand?: any;
    subBrand?: any;
    campaign?: any;
    
    id: number;

    quantity:number;
    isSelected:boolean;
    adjustedQuantity:number;
    availableQuantity:number;
}

export interface IWarehouseProductStock{
    wareHouseId: number;
    wareHouse?: any;
    posmProductId: number;
    posmProduct: IPosmProductStock;
    quantity: number;
    createdBy: number;
    createdTime: Date;
    modifiedBy: number;
    modifiedTime: Date;
    status: number;
    id: number;
}


export interface IWareHouse {
    name: string;
    code: string;
    type: number;
    contactPersonName: string;
    contactPhone: string;
    id: number;
    createdBy: number;
    createdTime: Date;
    modifiedBy: number;
    modifiedTime: Date;
    status: number;
}



