import { Status } from "src/app/Shared/Enums/status";

export class CampaignHistory {
    public id: number;
    public campaignId: number;
    public startDate: Date;
    public endDate: Date;
    public status: number;

    constructor() {
        this.id = 0;
        this.campaignId = 0;
        this.status = Status.Active;
    }
}