import { TWStatus } from "../../Enums/TWStatus";
import { Role } from "../Users";
import { UserInfo } from "../Users/userInfo";

export class TransactionWorkflow {
    
    id: number;
    workFlowId: number;
    twStatus: TWStatus;
    transactionId: number;
    userId: number;
    user?: UserInfo;
    roleId: number;
    role?: Role;
    sequence: number;
    workflowConfigurationId: number;
   

}