import { PosmInstallationStatus } from '../../Enums/posm-installation-status.enum';

export class DailyPosm {
    public id: number;
    public isPOSMInstallation: boolean;
    public isPOSMRepair: boolean;
    public isPOSMRemoval: boolean;
    public dailyCMActivityId: number;

    public posmInstallationStatus: string;
    public posmRepairStatus: string;
    public posmRemovalStatus: string;
    

    public  posmRemovalIncompleteReason: string;

    public  posmInstallationIncompleteReason: string;

    public posmRepairIncompleteReason : string;

    constructor() {

        this.posmInstallationStatus = Object.keys(PosmInstallationStatus).filter(k => isNaN(Number(k)))[0];
        this.posmRepairStatus = Object.keys(PosmInstallationStatus).filter(k => isNaN(Number(k)))[0];
        this.posmRemovalStatus = Object.keys(PosmInstallationStatus).filter(k => isNaN(Number(k)))[0];

    }

}
