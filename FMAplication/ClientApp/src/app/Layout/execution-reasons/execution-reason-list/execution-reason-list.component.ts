import { Component, OnInit } from '@angular/core';
import { Reason } from 'src/app/Shared/Entity/ExecutionReason/ExecutionReason';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { colDef, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { ExecutionReasonService } from 'src/app/Shared/Services/Reasons/execution-reason.service';
import { ReasonTableData } from 'src/app/Shared/Entity/ExecutionReason/ReasonTableData';

@Component({
  selector: 'app-execution-reason-list',
  templateUrl: './execution-reason-list.component.html',
  styleUrls: ['./execution-reason-list.component.css']
})
export class ExecutionReasonListComponent implements OnInit {

    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    permissionGroup: PermissionGroup = new PermissionGroup();
    reasonTableData:ReasonTableData[]=[];

    constructor(private reasonService: ExecutionReasonService, private alertService: AlertService, private router: Router,
        private activatedRoute: ActivatedRoute, private activityPermissionService: ActivityPermissionService) {
            this.initPermissionGroup();
    }

    ngOnInit() {
        this.fnGetExecutionReasonList();
    }

    private initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
    }

    public executionReasonList: Reason[] = [];
    private fnGetExecutionReasonList() {
        this.alertService.fnLoading(true);
        this.reasonService.getAllExecutionReasons().subscribe(
            (res) => {
                this.executionReasonList = res.data || [];

                // let executionReasonData = res.data || [];                
                // executionReasonData.forEach(obj => {
                //     obj.statusText = this.enumStatusTypes.filter(k => k.id == obj.status)[0].label;
                // });
                // this.executionReasonList = executionReasonData;
                // console.log(this.executionReasonList);
                this.mapToTable();
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }

    private fnRouteAddReason() {
        this.router.navigate(['/configuration/execution-reasons/execution-reason-add']);
    }

    private edit(id: number) {
        this.router.navigate(['/configuration/execution-reasons/execution-reason-add/' + id]);
    }

    private delete(id: number) {
        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.reasonService.deleteExecutionReason(id).subscribe(
                (res: any) => {
                    console.log('res from del func', res);
                    this.alertService.tosterSuccess("Execution Reason has been deleted successfully.");
                    this.fnGetExecutionReasonList();
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

    public ptableSettings: IPTableSetting<colDef<keyof ReasonTableData>> = {
        tableID: "Execution-Reasons-table",
        tableClass: "table table-border ",
        tableName: 'Non Execution Reason List',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'Reason Code', width: '15%', internalName: 'name', sort: true, type: "" },
            { headerName: 'Description In English', width: '25%', internalName: 'reasonInEnglish', sort: true, type: "" },
            { headerName: 'Description In Bangla', width: '25%', internalName: 'reasonInBangla', sort: true, type: "" },
            { headerName: 'Type', width: '20%', internalName: 'type', sort: true, type: "" },
            { headerName: 'Status', width: '10%', internalName: 'status', sort: true, type: "" }
        ],
        enabledSearch: true,
        enabledSerialNo: true,
        pageSize: 10,
        enabledPagination: true,
        //enabledAutoScrolled:true,
        // enabledEditDeleteBtn: true,
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
        enabledEditBtn: true,
        enabledDeleteBtn: true,
        // enabledTotal:true,
    };

    public fnCustomTrigger(event) {
        console.log("custom  click: ", event);

        if (event.action == "new-record") {
            this.fnRouteAddReason();
        }
        else if (event.action == "edit-item") {
            this.edit(event.record.id);
        }
        else if (event.action == "delete-item") {
            this.delete(event.record.id);
        }
    }

    mapToTable(){
        let tableData:ReasonTableData[] = [];
        for(let reason of this.executionReasonList) {
            let row = new ReasonTableData();
            row.id= reason.id;
            row.name = reason.name;
            row.reasonInBangla = reason.reasonInBangla;
            row.reasonInEnglish = reason.reasonInEnglish;
            if(reason.reasonReasonTypeMappings){
                 row.type = reason.reasonReasonTypeMappings.filter(x=>!!x.reasonType && x.reasonType.text != "Av").map(x=>x.reasonType.text).join(", ");
            }
            let statusType = this.enumStatusTypes.find(k => k.id == reason.status);
            if(statusType) row.status = statusType.label;            
            tableData.push(row);
        }
        this.reasonTableData = tableData;

    }

}
