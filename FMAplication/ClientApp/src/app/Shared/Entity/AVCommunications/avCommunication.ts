import { MapObject } from "../../Enums/mapObject";
import { IPtableMasterData } from "../../Modules/p-table";
import { Brand } from "../Brands";

export class AvCommunication {
    public id: number; 
    public campaignType : CampaignType;
    public campaignName : string;
    public description: string;

    public brandId: number;
    public brandModel: BrandModel;
    public file:File;
    public filePath: string;
    public isEditable:boolean;
    public isDeletable:boolean;


    constructor() {
        this.id = 0;
        
        this.description = "";
        this.isEditable=true;
        // this.brand = new Brand();
    }
}

export class AvCommunicationTable implements IPtableMasterData{
    public id: number; 
    public campaignName : string;
    public campaignType : string;
    public description: string;
    public brand: string;
    public filePath: string;
    disableDelete:boolean;
    disableEdit:boolean;
    downloadButtonText : string = "Download"
}

export enum CampaignType {
    Video,
    Image
}


export class AvCommunicationCampaignType {

    public static CampaignType: MapObject[] = [
        { id: 0, label: "Video" },
        { id: 1, label: "Image" },
    ];
}


export interface BrandModel {
    id: number;
    name: string;
    code: string;
    details?: any;
    status: number;
}


