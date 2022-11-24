import { Component, OnInit } from '@angular/core';
import { BrandService } from 'src/app/Shared/Services/Brand/brand.service';
import { Brand } from 'src/app/Shared/Entity/brands/brand';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from "../../../Shared/Enums/status";
import { AlertService } from "../../../Shared/Modules/alert/alert.service";
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { NgbCalendar, NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-brand-add',
    templateUrl: './brand-add.component.html',
    styleUrls: ['./brand-add.component.css']
})
export class BrandAddComponent implements OnInit {

    campModel: Brand = new Brand();
    enumStatusTypes: MapObject[] = StatusTypes.statusType;

    constructor(private alertService: AlertService, 
        private route: ActivatedRoute, 
        private brandService: BrandService, 
        private router: Router) { }
    ngOnInit() {
        this.createForm();
        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let productId = this.route.snapshot.params.id;
            this.getBrand(productId);
        }
    }

    createForm() {
    }
    
    public fnRouteCampList() {
        this.router.navigate(['/brand/brand-list']);
    }

    private getBrand(productId) {
        this.brandService.getBrand(productId).subscribe(
            (result: any) => {
                console.log("brand data", result.data);
                this.editForm(result.data);
            },
            (err: any) => console.log(err)
        );
    };

    private editForm(brand: Brand) {
        this.campModel.id = brand.id;
        this.campModel.name = brand.name;
        this.campModel.code = brand.code;
        this.campModel.details = brand.details;
        this.campModel.status = brand.status;
        
        console.log("brand data edit after", this.campModel);
    }

    public fnSaveBrand() {

        this.campModel.id == 0 ? this.insertBrand(this.campModel) : this.updateBrand(this.campModel);
    }

    private insertBrand(model: Brand) {
        model.name = model.name.trim();
        model.code = model.code.trim();
        this.brandService.postBrand(model).subscribe(res => {
            console.log("Brand res: ", res);
            this.router.navigate(['/brand/brand-list']).then(() => {
                this.alertService.tosterSuccess("Brand has been created successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private updateBrand(model: Brand) {
        model.name = model.name.trim();
        model.code = model.code.trim();
        this.brandService.putBrand(model).subscribe(res => {
            console.log("Brand upd res: ", res);
            this.router.navigate(['/brand/brand-list']).then(() => {
                this.alertService.tosterSuccess("Brand has been edited successfully.");
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
