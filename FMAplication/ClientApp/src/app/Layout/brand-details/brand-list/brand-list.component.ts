import { Component, OnInit } from '@angular/core';
import { BrandService } from 'src/app/Shared/Services/Brand/brand.service';
import { Brand } from 'src/app/Shared/Entity/Brands/brand';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from 'src/app/Shared/Enums/status';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { PermissionGroup, ActivityPermissionService } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';

@Component({
    selector: 'app-brand-list',
    templateUrl: './brand-list.component.html',
    styleUrls: ['./brand-list.component.css']
})
export class BrandListComponent implements OnInit {
    public brandList:Brand[]=[];
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    permissionGroup: PermissionGroup = new PermissionGroup();

    constructor(private brandService: BrandService,
         private alertService: AlertService,
         private activityPermissionService: ActivityPermissionService,
         private activatedRoute: ActivatedRoute,
          private router: Router) {
            this.initPermissionGroup();
    }
    
    ngOnInit() {
        this.fnGetBrandList();
    }


    private initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        console.log(this.permissionGroup);
        this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
    }

    private fnGetBrandList() {
        this.alertService.fnLoading(true);
        this.brandService.getBrandList().subscribe(
            (res) => {
                
                let brandsData = res.data.model || [];
                brandsData.forEach(obj => {
                    obj.statusText = this.enumStatusTypes.filter(k => k.id == obj.status)[0].label;
                });
                console.log("brandsData", brandsData);
                this.brandList = brandsData;
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }

    private fnRouteAddCamp() {
        this.router.navigate(['/brand/brand-add']);
    }

    private edit(id: number) {
        console.log('edit brand',id);
        this.router.navigate(['/brand/brand-add/' + id]);
    }

    private delete(id: number) {
        // console.log("Id:", id);
        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.brandService.deleteBrand(id).subscribe(
                (res: any) => {
                    console.log('res from del func',res);          
                    this.alertService.tosterSuccess("Brand has been deleted successfully.");
                    this.fnGetBrandList();
                },
                (error) => {
                    console.log(error);
                }
            );
        }, () => {

        });
    }

    public ptableSettings = {
        tableID: "Brands-table",
        tableClass: "table table-border ",
        tableName: 'Brands List',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'Brand Name ', width: '30%', internalName: 'name', sort: true, type: "" },
            { headerName: 'Brand Code ', width: '20%', internalName: 'code', sort: true, type: "" },
            { headerName: 'Brand Details ', width: '40%', internalName: 'details', sort: true, type: "" },
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

