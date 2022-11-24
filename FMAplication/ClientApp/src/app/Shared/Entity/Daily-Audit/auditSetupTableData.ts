import { IPTableSetting, colDef, IPtableMasterData } from 'src/app/Shared/Modules/p-table';

export class AuditSetupTableData implements IPtableMasterData{
    auditSetupId:number;
    salesPointCode:string;
    salesPointName:string;
    startDate:string;
    endDate:string;
    userType:string;
    auditType:string;
    productCode:string;    
    status:string;
    disableCheck: boolean | false;
    disableEdit: boolean | false;
    disableDelete: boolean | false;
    disableView: boolean | true;
}