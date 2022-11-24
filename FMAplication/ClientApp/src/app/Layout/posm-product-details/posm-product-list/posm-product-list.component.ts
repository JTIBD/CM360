import { Component, OnInit } from '@angular/core';
import { PosmProductService } from 'src/app/Shared/Services/Product/posmproduct.service';
import { PosmProduct } from 'src/app/Shared/Entity/Products/posmproduct';
import { Router, ActivatedRoute } from '@angular/router';
import { Status } from 'src/app/Shared/Enums/status';
import { PosmProductType } from 'src/app/Shared/Enums/posmproducttype';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';

@Component({
    selector: 'app-posm-product-list',
    templateUrl: './posm-product-list.component.html',
    styleUrls: ['./posm-product-list.component.css']
})

export class PosmProductListComponent implements OnInit {

    enumPosmProductType: MapObject[] = PosmProductType.posmProductType;
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    permissionGroup: PermissionGroup = new PermissionGroup();

    constructor(private posmProductService: PosmProductService, private alertService: AlertService, private router: Router,
        private activatedRoute: ActivatedRoute, private activityPermissionService: ActivityPermissionService) {
            this.initPermissionGroup();

    }

    ngOnInit() {
        this.fnGetPosmProductList();
    }


    private initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
    }

    public posmProductList: PosmProduct[] = [];
    private fnGetPosmProductList() {
        this.alertService.fnLoading(true);
        this.posmProductService.getPosmProductList().subscribe(
            (res) => {
                let posmProductsData = res.data.model || [];
                posmProductsData.forEach((obj:any) => {
                    obj.statusText = this.enumStatusTypes.filter(k => k.id == obj.status)[0].label;
                    obj.yesnoIsJti = obj.isJTIProduct ? "Yes" : "No";
                    obj.yesnoDigiSig = obj.isDigitalSignatureEnable ? "Yes" : "No";
                    obj.posmProdType = this.enumPosmProductType.filter(k => k.id == obj.type)[0].label;
                });
                this.posmProductList = posmProductsData;
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }

    private fnRouteAddPosmProd() {
        this.router.navigate(['/posm-product/posm-product-add']);
    }

    private edit(id: number) {
        this.router.navigate(['/posm-product/posm-product-add/' + id]);
    }

    private delete(id: number) {
        // console.log("Id:", id);
        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.posmProductService.deletePosmProduct(id).subscribe(
                (res: any) => {
                    console.log('res from del func', res);
                    this.alertService.tosterSuccess("POSM Product has been deleted successfully.");
                    this.fnGetPosmProductList();
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

    public ptableSettings: IPTableSetting = {
        tableID: "Products-table",
        tableClass: "table table-border ",
        tableName: 'POSM Products List',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'Product Code ', width: '10%', internalName: 'code', sort: true, type: "" },
            { headerName: 'Product Name ', width: '25%', internalName: 'name', sort: true, type: "" },
            { headerName: 'Product Type ', width: '15%', internalName: 'posmProdType', sort: true, type: "" },
            { headerName: 'Is JTI Product?', width: '15%', internalName: 'yesnoIsJti', sort: true, type: "" },
            { headerName: 'Is Digital Signature Enable?', width: '20%', internalName: 'yesnoDigiSig', sort: true, type: "" },
            { headerName: 'Status', width: '15%', internalName: 'statusText', sort: true, type: "" }
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
