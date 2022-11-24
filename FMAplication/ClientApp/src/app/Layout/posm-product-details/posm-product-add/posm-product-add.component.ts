import { Component, OnInit } from '@angular/core';
import { PosmProductService } from 'src/app/Shared/Services/Product/posmproduct.service';
import { PosmProduct } from 'src/app/Shared/Entity/Products/posmproduct';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from "../../../Shared/Enums/status";
import { AlertService } from "../../../Shared/Modules/alert/alert.service";
import { MapObject } from "../../../Shared/Enums/mapObject";
import { WorkflowTypes } from "../../../Shared/Enums/WorkflowTypes";
import { PosmProductType } from "../../../Shared/Enums/posmproducttype";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { Brand, SubBrand } from 'src/app/Shared/Entity/Brands';
import { BrandService } from 'src/app/Shared/Services/Brand/brand.service';
import { SubBrandService } from 'src/app/Shared/Services/Brand/sub-brand.service';
import { Campaign } from 'src/app/Shared/Entity/Campaigns';
import { CampaignService } from 'src/app/Shared/Services/Campaign/campaign.service';

@Component({
    selector: 'app-posm-product-add',
    templateUrl: './posm-product-add.component.html',
    styleUrls: ['./posm-product-add.component.css']
})
export class PosmProductAddComponent implements OnInit {

    public posmProdModel: PosmProduct = new PosmProduct();
    enumPosmProductType: MapObject[] = PosmProductType.posmProductType;
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    brandList: Brand[] = [];
    subbrandList: SubBrand[] = [];
    campaignList: Campaign[] = [];

    constructor(private alertService: AlertService, private route: ActivatedRoute, private productService: PosmProductService, private router: Router,
        private brandService: BrandService, private subbrandService: SubBrandService, private campaignService: CampaignService) { }

    ngOnInit() {
        this.createForm();
        this.getBrandList();
        this.getCampaignList();
        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let productId = this.route.snapshot.params.id;
            this.getPosmProduct(productId);
        }
    }

    createForm() {
        // console.log("statusOptions: " , this.statusOptions, this.keys);
    }

    getCampaignList() {
        this.campaignService.getAllForSelect().subscribe(data => {
            this.campaignList = data.data;
        })
    }

    getBrandList() {
        this.brandService.getAllForSelect().subscribe(data => {
            this.brandList = data.data;
        })
    }

    getSubBrandList(event: any) {
        let brandId = event;
        console.log(event);
        this.subbrandService.getAllForSelect(brandId).subscribe(data => {
            this.subbrandList = data.data;
        })
    }

    public fnRoutePosmProdList() {
        this.router.navigate(['/posm-product/posm-product-list']);
    }

    private getPosmProduct(productId) {
        this.productService.getPosmProduct(productId).subscribe(
            (result: any) => {
                console.log("product data", result.data);
                this.editForm(result.data);
            },
            (err: any) => console.log(err)
        );
    };

    private editForm(product: PosmProduct) {
        this.posmProdModel.id = product.id;
        this.posmProdModel.name = product.name;
        this.posmProdModel.code = product.code;
        this.posmProdModel.isJTIProduct = product.isJTIProduct;
        this.posmProdModel.isDigitalSignatureEnable = product.isDigitalSignatureEnable;
        this.posmProdModel.type = product.type;
        this.posmProdModel.status = product.status;
        this.posmProdModel.isPlanogram = product.isPlanogram;
        this.posmProdModel.planogramImageUrl = product.planogramImageUrl;
        this.posmProdModel.imageUrl = product.imageUrl;
        this.posmProdModel.campaignId = product.campaignId;
        this.posmProdModel.brandId = product.brandId;
        this.posmProdModel.subBrandId = product.subBrandId;
        if(this.posmProdModel.brandId)
            this.getSubBrandList(this.posmProdModel.brandId);
        console.log("posm product data edit after", this.posmProdModel);
    }

    public fnSavePosmProduct(model: PosmProduct) {
        this.posmProdModel.id == 0 ? this.insertPosmProduct(this.posmProdModel) : this.updatePosmProduct(this.posmProdModel);
    }

    private insertPosmProduct(model: PosmProduct) {
        model.code = model.code.trim();
        this.productService.postPosmProduct(model).subscribe(res => {
            console.log("PosmProduct res: ", res);
            this.router.navigate(['/posm-product/posm-product-list']).then(() => {
                this.alertService.tosterSuccess("POSM Product has been created successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private updatePosmProduct(model: PosmProduct) {
        model.code = model.code.trim();
        this.productService.putPosmProduct(model).subscribe(res => {
            console.log("PosmProduct upd res: ", res);
            this.router.navigate(['/posm-product/posm-product-list']).then(() => {
                this.alertService.tosterSuccess("POSM Product has been edited successfully.");
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

        let errList = errorDetails.error.errors;
        if (errList.length) {
            console.log("error", errList, errList[0].errorList[0]);
            this.alertService.tosterDanger(errList[0].errorList[0]);
        } else {
            this.alertService.tosterDanger(errorDetails.error.msg);
        }
    }

    onChangePlanogramFile(file: File) {        
        // const selectedFile = <File>file;

        console.log("image file", file);
        // console.log(selectedFile);
        this.posmProdModel.planogramImageFile = file;
        if(this.posmProdModel.planogramImageFile != null) 
        this.posmProdModel.planogramImageUrl = file.name;
    }

    onChangeImageFile(file: File) {
        // const selectedFile = <File>file;

        console.log("image file", file);
        // console.log(selectedFile);
        this.posmProdModel.imageFile = file;
        if(this.posmProdModel.imageFile != null) 
            this.posmProdModel.imageUrl = file.name;
    }

}
