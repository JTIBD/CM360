import { Status } from '../../Enums/status';
import { WorkFlowConfiguration } from './workflowconfiguration';

export class WorkFlowConfigurepTableData {

        public  id : number;
        public workflowType : number;
        public workflowTypeName : string;
        public workflowConfigType:number;
        public workflowConfigTypeName:string;
        public workflowStep : number;

        public  code : string;
        public configList : WorkFlowConfiguration[];
        public status : number;
        public approver:string;
        

        constructor()
        {
            this.status = Status.Active;
            this.configList = [];
        }
}