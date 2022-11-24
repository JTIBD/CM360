import { RoutesLaout } from "./RoutesLaout";

export class RoutesExecutionLimit{
    static Parent = RoutesLaout.Configuration;    
    static MinimumExecutionLimit = 'execution-limit';
    static EditExecutionLimit = 'edit-execution-limit';
    static NewExecutionLimit = 'new-execution-limit';    
}