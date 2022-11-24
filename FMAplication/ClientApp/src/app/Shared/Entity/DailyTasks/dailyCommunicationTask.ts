import { CommunicationSetup } from "../AVCommunications/communicationSetup";
import { DailyBaseTask } from "./dailyBaseTask";

export class DailyCommunicationTask extends DailyBaseTask{    
    communicationSetupId: number;
    communicationSetup: CommunicationSetup;    
}