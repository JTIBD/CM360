
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkflowconfigurationService } from "src/app/Shared/Services/Workflow/workflowconfiguration.service";
import { WorkFlowConfiguration } from "src/app/Shared/Entity/WorkFlows/workflowconfiguration";
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { WorkFlowConfigurepTableData } from 'src/app/Shared/Entity/WorkFlows/workflowConfigurepTableData';

@Component({
    selector: 'app-workflow-configuration-list',
    templateUrl: './workflow-configuration-list.component.html',
    styleUrls: ['./workflow-configuration-list.component.css']
})


export class WorkflowConfigurationListComponent implements OnInit {

  

    workflowTable:WorkFlowConfigurepTableData[] = []; 
    permissionGroup: PermissionGroup = new PermissionGroup();

    constructor(private workflowconfigurationService: WorkflowconfigurationService,
         private alertService: AlertService,
         private router: Router,
         private activatedRoute: ActivatedRoute,
         private activityPermissionService: ActivityPermissionService) {
            this.initPermissionGroup();

    }

    ngOnInit() {
        this.fnGetWorkflowconfigurationList();
    }

    private initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
    }

    public workflowconfigurationList: WorkFlowConfiguration[] = [];
    private fnGetWorkflowconfigurationList() {
        this.alertService.fnLoading(true);
        this.workflowconfigurationService.getWorkFlowConfigurationList().subscribe(
            (res) => {
                const result = res.data;
                this.mapToPTable(result);
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }


    mapToPTable(result) {
       
        this.workflowTable = result.map(x => {
            const data = new WorkFlowConfigurepTableData();
            data.id = x.id;
            data.code = x.code;
            data.workflowTypeName = x.name;
            data.workflowStep = x.workflowStep;
            data.workflowConfigType = x.workflowConfigType; 
            data.workflowConfigTypeName = x.workflowConfigType === 1 ? "User" : "Role";
            data.status = x[x.status];
            data.approver = '';
            const configList = x.configList;
            configList.forEach(item => {
                if (data.workflowConfigType === 1)
                    data.approver += item.user.name + " "; 
                if (data.workflowConfigType === 2)
                    data.approver += item.role.name + " "; 
            });
            return data;
          });

    }
    private fnRouteAddPosmProd() {
        this.router.navigate(['/work-flow/workflow-configuration-add']);
    }

    private edit(id: number) {
        this.router.navigate(['/work-flow/workflow-configuration-add/' + id]);
    }

    private delete(id: number) {
       
        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.workflowconfigurationService.deleteWorkFlowConfiguration(id).subscribe(
                (res: any) => {
                  
                    this.alertService.tosterSuccess("Workflow Configuration has been deleted successfully.");
                    this.fnGetWorkflowconfigurationList();
                },
                (error) => {
                    console.log(error);
                }
            );
        }, () => {

        });
    }

    public fnPtableCellClick(event) {
        console.log("cell click: ");
    }

    public ptableSettings = {
        tableID: "Products-table",
        tableClass: "table table-border ",
        tableName: 'WorkFlow Configuration List',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'Code', width: '20%', internalName: 'code', sort: true, type: "" },
            { headerName: 'WorkFlow ', width: '20%', internalName: 'workflowTypeName', sort: true, type: "" },
            { headerName: 'Type ', width: '20%', internalName: 'workflowConfigTypeName', sort: true, type: "" },
            { headerName: 'Steps ', width: '20%', internalName: 'workflowStep', sort: true, type: "" },
            { headerName: 'Approver', width: '20%', internalName: 'approver', sort: false, type: "" }
        ],
        enabledSearch: true,
        enabledSerialNo: true,
        pageSize: 10,
        enabledPagination: true,
        //enabledAutoScrolled:true,
        enabledEditBtn: true,
        enabledDeleteBtn: true,
        //enabledEditDeleteBtn: true,
        // enabledCellClick: true,
        enabledColumnFilter: true,
        // enabledDataLength:true,
        // enabledColumnResize:true,
        // enabledReflow:true,
        // enabledPdfDownload:true,
        // enabledExcelDownload:true,
        // enabledPrint:true,
        // enabledColumnSetting:true,
        enabledRecordCreateBtn: true,
        // enabledTotal:true,
    };

    public fnCustomTrigger(event) {
        console.log("custom  click: ", event);

        if (event.action == "new-record") {
            this.fnRouteAddPosmProd();
        }
        else if (event.action == "edit-item") {
            this.edit(event.record.id);
        }
        else if (event.action == "delete-item") {
            this.delete(event.record.id);
        }
    }

}
