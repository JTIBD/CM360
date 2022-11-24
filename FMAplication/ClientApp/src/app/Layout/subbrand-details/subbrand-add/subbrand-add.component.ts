import { Component, OnInit } from '@angular/core';
import { SubBrandService } from 'src/app/Shared/Services/Brand/sub-brand.service';
import { SubBrand } from 'src/app/Shared/Entity/brands/sub-brand';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from "../../../Shared/Enums/status";
import { AlertService } from "../../../Shared/Modules/alert/alert.service";
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { NgbCalendar, NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { Brand } from 'src/app/Shared/Entity/Brands';
import { BrandService } from 'src/app/Shared/Services/Brand/brand.service';

@Component({
    selector: 'app-subbrand-add',
    templateUrl: './subbrand-add.component.html',
    styleUrls: ['./subbrand-add.component.css']
})
export class SubBrandAddComponent implements OnInit {
    brandList: Brand[] = [];
    campModel: SubBrand = new SubBrand();
    enumStatusTypes: MapObject[] = StatusTypes.statusType;

    constructor(private alertService: AlertService, 
        private route: ActivatedRoute, 
        private subbrandService: SubBrandService, 
        private brandService: BrandService, 
        private router: Router) { }
    ngOnInit() {
        this.getBrandList();
        this.createForm();
        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let productId = this.route.snapshot.params.id;
            this.getSubBrand(productId);
        }
    }

    createForm() {
    }

    getBrandList() {
        this.brandService.getAllForSelect().subscribe(data => {
            this.brandList = data.data;
        })
    }
    
    public fnRouteCampList() {
        this.router.navigate(['/subbrand/subbrand-list']);
    }

    private getSubBrand(productId) {
        this.subbrandService.getSubBrand(productId).subscribe(
            (result: any) => {
                console.log("subbrand data", result.data);
                this.editForm(result.data);
            },
            (err: any) => console.log(err)
        );
    };

    private editForm(subbrand: SubBrand) {
        this.campModel.id = subbrand.id;
        this.campModel.brandId = subbrand.brandId;
        this.campModel.name = subbrand.name;
        this.campModel.code = subbrand.code;
        this.campModel.details = subbrand.details;
        this.campModel.status = subbrand.status;
        
        console.log("subbrand data edit after", this.campModel);
    }

    public fnSaveSubBrand() {

        this.campModel.id == 0 ? this.insertSubBrand(this.campModel) : this.updateSubBrand(this.campModel);
    }

    private insertSubBrand(model: SubBrand) {
        model.name = model.name.trim();
        model.code = model.code.trim();
        this.subbrandService.postSubBrand(model).subscribe(res => {
            console.log("SubBrand res: ", res);
            this.router.navigate(['/subbrand/subbrand-list']).then(() => {
                this.alertService.tosterSuccess("SubBrand has been created successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private updateSubBrand(model: SubBrand) {
        model.name = model.name.trim();
        model.code = model.code.trim();
        this.subbrandService.putSubBrand(model).subscribe(res => {
            console.log("SubBrand upd res: ", res);
            this.router.navigate(['/subbrand/subbrand-list']).then(() => {
                this.alertService.tosterSuccess("SubBrand has been edited successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private displayError(errorDetails: any) {
        // this.alertService.fnLoading(false);
        console.log("error", errorDetails);
        let errList = errorDetails.error.errors;
        if (errList.length) {
            console.log("error", errList, errList[0].errorList[0]);
            this.alertService.tosterDanger(errList[0].errorList[0]);
        } else {
            this.alertService.tosterDanger(errorDetails.error.msg);
        }
    }

}
