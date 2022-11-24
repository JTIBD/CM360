import { Component, OnInit } from '@angular/core';
import { SubBrandService } from 'src/app/Shared/Services/Brand/sub-brand.service';
import { SubBrand } from 'src/app/Shared/Entity/Brands/sub-brand';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from 'src/app/Shared/Enums/status';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { PermissionGroup, ActivityPermissionService } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';

@Component({
    selector: 'app-subbrand-list',
    templateUrl: './subbrand-list.component.html',
    styleUrls: ['./subbrand-list.component.css']
})
export class SubBrandListComponent implements OnInit {
    public subbrandList:SubBrand[]=[];
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    permissionGroup: PermissionGroup = new PermissionGroup();

    constructor(private subbrandService: SubBrandService,
         private alertService: AlertService,
         private activityPermissionService: ActivityPermissionService,
         private activatedRoute: ActivatedRoute,
          private router: Router) {
            this.initPermissionGroup();
    }
    
    ngOnInit() {
        this.fnGetSubBrandList();
    }


    private initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        console.log(this.permissionGroup);
        this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
    }

    private fnGetSubBrandList() {
        this.alertService.fnLoading(true);
        this.subbrandService.getSubBrandList().subscribe(
            (res) => {
                let subbrandsData = res.data.model || [];
                subbrandsData.forEach(obj => {
                    obj.statusText = this.enumStatusTypes.filter(k => k.id == obj.status)[0].label;
                    obj.brandName = obj.brand != null ? obj.brand.name : '';
                });
                console.log("subbrandsData", subbrandsData);
                this.subbrandList = subbrandsData;
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }

    private fnRouteAddCamp() {
        this.router.navigate(['/subbrand/subbrand-add']);
    }

    private edit(id: number) {
        console.log('edit subbrand',id);
        this.router.navigate(['/subbrand/subbrand-add/' + id]);
    }

    private delete(id: number) {
        // console.log("Id:", id);
        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.subbrandService.deleteSubBrand(id).subscribe(
                (res: any) => {
                    console.log('res from del func',res);          
                    this.alertService.tosterSuccess("SubBrand has been deleted successfully.");
                    this.fnGetSubBrandList();
                },
                (error) => {
                    console.log(error);
                }
            );
        }, () => {

        });
    }

    public ptableSettings = {
        tableID: "SubBrands-table",
        tableClass: "table table-border ",
        tableName: 'SubBrands List',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'SubBrand Name ', width: '25%', internalName: 'name', sort: true, type: "" },
            { headerName: 'SubBrand Code ', width: '15%', internalName: 'code', sort: true, type: "" },
            { headerName: 'SubBrand Details ', width: '30%', internalName: 'details', sort: true, type: "" },
            { headerName: 'Brand Name ', width: '20%', internalName: 'brandName', sort: true, type: "" },
            { headerName: 'Status', width: '10%', internalName: 'statusText', sort: true, type: "" }
        ],
        enabledSearch: true,
        enabledSerialNo: true,
        pageSize: 10,
        enabledPagination: true,
        //enabledAutoScrolled:true,
        enabledDeleteBtn: true,
        enabledEditBtn: true,
        enabledCellClick: false,
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

