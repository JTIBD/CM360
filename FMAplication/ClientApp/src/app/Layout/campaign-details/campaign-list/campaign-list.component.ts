import { Component, OnInit } from '@angular/core';
import { CampaignService } from 'src/app/Shared/Services/Campaign/campaign.service';
import { Campaign } from 'src/app/Shared/Entity/Campaigns/campaign';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from 'src/app/Shared/Enums/status';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { PermissionGroup, ActivityPermissionService } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';

@Component({
    selector: 'app-campaign-list',
    templateUrl: './campaign-list.component.html',
    styleUrls: ['./campaign-list.component.css']
})
export class CampaignListComponent implements OnInit {
    public campaignList:Campaign[]=[];
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    permissionGroup: PermissionGroup = new PermissionGroup();

    constructor(private campaignService: CampaignService,
         private alertService: AlertService,
         private activityPermissionService: ActivityPermissionService,
         private activatedRoute: ActivatedRoute,
          private router: Router) {
            this.initPermissionGroup();
    }
    
    ngOnInit() {
        this.fnGetCampaignList();
    }


    private initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        console.log(this.permissionGroup);
        this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
    }

    private fnGetCampaignList() {
        this.alertService.fnLoading(true);
        this.campaignService.getCampaignList().subscribe(
            (res) => {
                let campaignsData = res.data.model || [];
                campaignsData.forEach(obj => {
                    obj.statusText = this.enumStatusTypes.filter(k => k.id == obj.status)[0].label;
                    obj.startDateText = this.dateToString(obj.startDate);
                    obj.endDateText = this.dateToString(obj.endDate);
                    obj.detailBtn = 'Details';
                });
                console.log("campaignsData", campaignsData);
                this.campaignList = campaignsData;
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }

    dateToString(date: Date): string {
        let newDate = new Date(date);
        return `${newDate.getFullYear()}-${newDate.getMonth()+1}-${newDate.getDate()}`;
    }

    private fnRouteAddCamp() {
        this.router.navigate(['/campaign/campaign-add']);
    }

    private edit(id: number) {
        console.log('edit campaign',id);
        this.router.navigate(['/campaign/campaign-add/' + id]);
    }

    private delete(id: number) {
        // console.log("Id:", id);
        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.campaignService.deleteCampaign(id).subscribe(
                (res: any) => {
                    console.log('res from del func',res);          
                    this.alertService.tosterSuccess("Campaign has been deleted successfully.");
                    this.fnGetCampaignList();
                },
                (error) => {
                    console.log(error);
                }
            );
        }, () => {

        });
    }

    public fnPtableCellClick(event: any) {
        console.log("cell click: ", event);
        this.router.navigate(['/campaign/campaign-details/' + event.record.id]);
    }

    public ptableSettings = {
        tableID: "Campaigns-table",
        tableClass: "table table-border ",
        tableName: 'Campaigns List',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'Campaign Name ', width: '15%', internalName: 'campaignName', sort: true, type: "" },
            { headerName: 'Campaign Details ', width: '25%', internalName: 'campaignDetails', sort: true, type: "" },
            { headerName: 'Start Date ', width: '20%', internalName: 'startDateText', sort: true, type: "" },
            { headerName: 'EndDate', width: '20%', internalName: 'endDateText', sort: true, type: "" },
            { headerName: 'Status', width: '10%', internalName: 'statusText', sort: true, type: "" },
            { headerName: 'Detail', width: '10%', internalName: 'detailBtn', sort: false, type: "button", onClick: 'true', innerBtnIcon:"fa fa-info" }
        ],
        enabledSearch: true,
        enabledSerialNo: true,
        pageSize: 10,
        enabledPagination: true,
        //enabledAutoScrolled:true,
        enabledDeleteBtn: true,
        enabledEditBtn: true,
        enabledCellClick: true,
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
            this.fnRouteAddCamp();
        }
        else if (event.action == "edit-item") {
            this.edit(event.record.id);
        }
        else if (event.action == "delete-item") {
            this.delete(event.record.id);
        }
    }

}

