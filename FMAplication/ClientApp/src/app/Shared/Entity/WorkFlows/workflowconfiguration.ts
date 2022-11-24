import { Status } from "../../Enums/status";

export class WorkFlowConfiguration {
    public id: number;
    
    public roleId: number;
    public role: any;

    public userId:number;
    public user:any;

    public masterWorkFlowId: number;
    public masterWorkFlowName: string;

    public status: number;
    public typeIds : number[]; 
    public sequence:number;
    
    constructor() {
        this.id = 0;
        this.status = Status.Active;
    }
}