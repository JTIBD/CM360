import { DailyBaseTask } from "./dailyBaseTask";
import { DailyPosmTaskItem } from "./dailyPosmTaskItem";

export class DailyPosmTask extends DailyBaseTask{
    existingImage: string;
    newImage: string;
    dailyPosmTaskItems: DailyPosmTaskItem[];
}