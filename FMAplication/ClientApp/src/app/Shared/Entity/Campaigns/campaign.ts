import { Status } from "src/app/Shared/Enums/status";
import { CampaignHistory } from './campaign-history';

export class Campaign {
    public id: number;
    public campaignName: string;
    public campaignDetails: string;
    public startDate: Date;
    public endDate: Date;
    public status: number;

    public campaignHistories: CampaignHistory[];

    constructor() {
        this.id = 0;
        this.campaignName = '';
        this.campaignDetails = '';
        this.status = Status.Active;

        this.campaignHistories = [];
    }
}