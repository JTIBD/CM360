export enum TaskStatus {
    Confirmed,
    Downloaded,
    Uploaded, 
    Completed
}


export class PosmAssign {
    id: number;
    salesPointId: number;
    salesPoint: SalesPoint;
    posmProductId: number;
    posmProduct?: any;
    cmUserId: number;
    cmUser: CmUser;
    taskStatus: TaskStatus;
    date: string;
    dateStr: string;
    quantity: number;
    sumOfQuantity: number;
    lines: number;
}

export class SalesPoint {
    salesPointId: number;
    code: string;
    name: string;
    banglaName: string;
    officeAddress: string;
    contactNo: string;
    emailAddress: string;
    deliveryChannelType: number;
    latitude?: any;
    longitude: string;
    sequenceId: number;
    townName: string;
    depotCode: number;
    officeAddressBangla: string;
}

export class CmUser {
    id: number;
    name: string;
    code: string;
}

export class PosmAssignTableName {
    id: number;
    salesPointId: number;
    salesPointName: string;
    posmProductId: number;
    posmProduct?: any;
    cmUserId: number;
    cmUser: string;
    taskStatus: TaskStatus;
    date: string;
    quantity: number;
    sumOfQuantity: number;
    lines: number;
    status:string;
}

export interface PosmProduct {
    id: number;
    name: string;
    type: number;
    isJTIProduct: boolean;
    status: number;
    isDigitalSignatureEnable: boolean;
    isPlanogram: boolean;
    planogramImageUrl: string;
    code: string;
    planogramImageFile?: any;
    brandId?: any;
    subBrandId?: any;
    campaignId: number;
}

export interface PosmAssignDetails {
    posmProduct: PosmProduct;
    quantity: number;
}

export class PosmAssignDetailsTable {
    posmProductCode:string; 
    posmProductName: string;
    quantity: number;
}

export class PosmParams { 
    constructor() {
        this.pageIndex = 1; 
        this.pageSize = 10;
        this.search = "";
        this.salesPointId = 0;
        
        let date = new Date();
        this.fromDate = new Date(date.getFullYear(), date.getMonth() , 1).toISOString();
        this.toDate=  new Date().toISOString();
        
      }
      pageSize:number; 
      pageIndex:number;
      salesPointId: number;
      search:string;
      sort : string;
      fromDate:string; 
      toDate:string;
}