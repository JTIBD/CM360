import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/Shared/Services/Product/product.service';
import { Product } from 'src/app/Shared/Entity/Products/product';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from "../../../Shared/Enums/status";
import { AlertService } from "../../../Shared/Modules/alert/alert.service";
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { SubBrandService } from 'src/app/Shared/Services/Brand/sub-brand.service';
import { BrandService } from 'src/app/Shared/Services/Brand/brand.service';
import { Brand, SubBrand } from 'src/app/Shared/Entity/Brands';

@Component({
    selector: 'app-product-add',
    templateUrl: './product-add.component.html',
    styleUrls: ['./product-add.component.sass']
})
export class ProductAddComponent implements OnInit {

    prodModel: Product = new Product();
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    brandList: Brand[] = [];
    subbrandList: SubBrand[] = [];

    constructor(private alertService: AlertService, private route: ActivatedRoute, private productService: ProductService, private router: Router,
        private brandService: BrandService, private subbrandService: SubBrandService) { }
    ngOnInit() {
        this.createForm();
        this.getBrandList();
        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let productId = this.route.snapshot.params.id;
            this.getProduct(productId);
        }
    }

    createForm() {
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
    
    public fnRouteProdList() {
        this.router.navigate(['/product/product-list']);
    }

    private getProduct(productId) {
        this.productService.getProduct(productId).subscribe(
            (result: any) => {
                console.log("product data", result.data);
                this.editForm(result.data);
            },
            (err: any) => console.log(err)
        );
    };

    private editForm(product: Product) {
        this.prodModel.id = product.id;
        this.prodModel.name = product.name;
        this.prodModel.code = product.code;
        this.prodModel.isJTIProduct = product.isJTIProduct;
        this.prodModel.type = product.type;
        this.prodModel.status = product.status;
        this.prodModel.brandId = product.brandId;
        this.prodModel.imageUrl = product.imageUrl;
        this.prodModel.subBrandId = product.subBrandId;
        if(this.prodModel.brandId)
            this.getSubBrandList(this.prodModel.brandId);
        console.log("product data edit after", this.prodModel);
    }

    public fnSaveProduct() {
        this.prodModel.id == 0 ? this.insertProduct(this.prodModel) : this.updateProduct(this.prodModel);
    }

    private insertProduct(model: Product) {
        model.code = model.code.trim();
        this.productService.postProduct(model).subscribe(res => {
            console.log("Product res: ", res);
            this.router.navigate(['/product/product-list']).then(() => {
                this.alertService.tosterSuccess("Product has been created successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private updateProduct(model: Product) {
        model.code = model.code.trim();
        this.productService.putProduct(model).subscribe(res => {
            console.log("Product upd res: ", res);
            this.router.navigate(['/product/product-list']).then(() => {
                this.alertService.tosterSuccess("Product has been edited successfully.");
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

    onChangeFile(file: File) {
        // const selectedFile = <File>file;

        console.log("image file", file);
        // console.log(selectedFile);
        this.prodModel.imageFile = file;
        if (file != null) this.prodModel.imageUrl = file.name;
    }

}
