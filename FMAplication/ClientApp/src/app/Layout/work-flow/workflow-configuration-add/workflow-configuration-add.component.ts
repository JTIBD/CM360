import { RoleService } from 'src/app/Shared/Services/Users/role.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { Component, OnInit } from '@angular/core';
import { WorkflowconfigurationService } from "src/app/Shared/Services/Workflow/workflowconfiguration.service";
import { WorkFlowConfiguration } from "src/app/Shared/Entity/WorkFlows/workflowconfiguration";
import { ActivatedRoute, Router } from '@angular/router';
import { WorkFlowService } from "../../../Shared/Services/Workflow/work-flow.service";
import { OrganizationRoleService } from "../../../Shared/Services/Workflow/organizationrole.service";
import { ModeOfApprovals } from "../../../Shared/Enums/modeOfApproval";
import { ApprovalStatuses } from "../../../Shared/Enums/approvalStatus";
import { RejectedStatuses } from "../../../Shared/Enums/rejectedStatus";
import { NotificationStatuses } from "../../../Shared/Enums/notificationStatus";
import { AlertService } from "../../../Shared/Modules/alert/alert.service";
import { Status } from "../../../Shared/Enums/status";
import { OrganizationRole } from "../../../Shared/Entity/Organizations/orgrole";
import { WorkFlow } from 'src/app/Shared/Entity/WorkFlows/work-flow';
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { UserInfo } from 'src/app/Shared/Entity';


@Component({
    selector: 'app-workflow-configuration-add',
    templateUrl: './workflow-configuration-add.component.html',
    styleUrls: ['./workflow-configuration-add.component.css']
})
export class WorkflowConfigurationAddComponent implements OnInit {

    public workFlowConfigurationModel: WorkFlowConfiguration = new WorkFlowConfiguration();
    public workFlowList: WorkFlow[] = [];
    workFlowModel: WorkFlow;
    public organizationRoleList: OrganizationRole[] = [];
    sequences : any[] = []; 

    modeOfApprovalOptions: MapObject[] = ModeOfApprovals.modeOfApproval;
    approvalStatusOptions: MapObject[] = ApprovalStatuses.approvalStatus;
    rejectedStatusOptions: MapObject[] = RejectedStatuses.rejectedStatus;
    notificationStatusOptions: MapObject[] = NotificationStatuses.notificationStatus;
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    
    constructor(private alertService: AlertService, 
        private route: ActivatedRoute, 
        private workflowconfigurationService: WorkflowconfigurationService, 
        private workFlowService: WorkFlowService, 
        private roleService: RoleService,
        private router: Router, 
        private userService:UserService) { }

    ngOnInit() {
        this.fnGetWorkFlowList();
        this.fnGetOrganizationRoleList();
        this.getUserList();

        this.createForm();


        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            
            let workflowConfigId = this.route.snapshot.params.id;
            this.getWorkflowConfiguration(workflowConfigId);
        }
    }

    createForm() {
    }

 
    // steps(i: number) {
    //     return new Array(i);
    // }


  workFlowChange(){
      this.workFlowModel = null;
      this.sequences = [];
      this.workFlowModel = this.workFlowList.find(x=>x.id === this.workFlowConfigurationModel.masterWorkFlowId);
      for (let index = 1; index <= this.workFlowModel.workflowStep; index++) {
        this.sequences.push({id:index, value: null})
      }
      console.log(this.workFlowModel);
  }

    public fnRouteWorkFlowConfigurationList() {
        this.router.navigate(['/work-flow/workflow-configuration-list']);
    }

    private getWorkflowConfiguration(workflowConfigId) {
        this.workflowconfigurationService.getWorkFlowConfigurationById(workflowConfigId).subscribe(
            (result: any) => {
               
                this.editForm(result.data);
            },
            (err: any) => console.log(err)
        );
    };

    private editForm(workflow: WorkFlow) {
        this.workFlowModel = workflow;
        this.workFlowConfigurationModel.masterWorkFlowId = workflow.id;
        const configList = workflow.configList; 
        if (workflow.workflowConfigType === 1)
        {
            configList.forEach(config => {
                this.sequences.push({id:config.sequence, value:config.userId})
            });
        }
        else 
        {
            configList.forEach(config => {
                this.sequences.push({id:config.sequence, value:config.roleId})
            });
        }
        
        for(let startLength = this.sequences.length; startLength < workflow.workflowStep; startLength++){
            this.sequences.push({id:startLength+1, value:null})
        }
    }

    public fnSaveWorkFlowConfiguration() {
        var valueArr = this.sequences.map(function(item){ return item.value });
        var hasDuplicate = valueArr.some(function(item, idx){ return valueArr.indexOf(item) != idx});
    
        if(hasDuplicate) {
            this.alertService.tosterDanger("Please select different approver");
            return;
        }
        this.workFlowConfigurationModel.typeIds = valueArr;
        this.workFlowConfigurationModel.id == 0 ? this.insertWorkflowConfiguration(this.workFlowConfigurationModel) : this.updateWorkflowConfiguration(this.workFlowConfigurationModel);
    }

    private insertWorkflowConfiguration(model: WorkFlowConfiguration) {
        this.workflowconfigurationService.postWorkFlowConfiguration(model).subscribe(res => {
            
            this.router.navigate(['/work-flow/workflow-configuration-list']).then(() => {
                this.alertService.tosterSuccess("WorkFlow Configuration created successfully.");
            });
        },
            (error) => {
                
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private updateWorkflowConfiguration(model: WorkFlowConfiguration) {
        this.workflowconfigurationService.putWorkFlowConfiguration(model).subscribe(res => {
           
            this.router.navigate(['/work-flow/workflow-configuration-list']).then(() => {
                this.alertService.tosterSuccess("WorkFlow Configuration edited successfully.");
            });
        },
            (error) => {
               
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    public userInfoList: UserInfo[] = [];
    getUserList() {
    
        this.userService.getAllUserInfo().subscribe((result: any) => {
          //  console.log("user info list", res.data);
            this.userInfoList = result.data;
        });
    }

    changeUser(event, seq){
      
        console.log(this.sequences);

    }

    private displayError(errorDetails: any) {
        // this.alertService.fnLoading(false);
       
        let errList = errorDetails.error.errors;
        if (errList.length) {
            console.log("error", errList, errList[0].errorList[0]);
            this.alertService.tosterDanger(errList[0].errorList[0]);
        } else {
            this.alertService.tosterDanger(errorDetails.error.msg);
        }
    }

    private fnGetWorkFlowList() {
        this.workFlowService.getWorkFLowList().subscribe((res: any) => {
            this.workFlowList = res.data;
           
        });
    }

    private fnGetOrganizationRoleList() {
        this.roleService.getRoleList().subscribe((res: any) => {
            this.organizationRoleList = res.data.model;
           
        });
    }


    isValidForm() {
        const sequences = this.sequences.filter(x=>x.value != null); 
        const count =  sequences.length;
        return this.workFlowModel && this.workFlowModel.id > 0 && this.workFlowModel.workflowStep === count && count > 0;
    }
}
