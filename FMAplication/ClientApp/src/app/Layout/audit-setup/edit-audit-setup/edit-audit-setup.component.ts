import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import * as moment from 'moment';
import { AuditPOSMProduct, AuditProduct, AuditSetup, MappedProduct } from 'src/app/Shared/Entity/Daily-Audit';
import { DailyAuditService } from 'src/app/Shared/Services/DailyActivity/daily-audit.service';
import { RoutesAudit } from '../routesAudit';
import { DailyCMActivity, Product, PosmProduct, APIResponsePage } from 'src/app/Shared/Entity';
import { colDef, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { finalize } from 'rxjs/operators';
import { CmTaskGenerationService } from 'src/app/Shared/Services/DailyActivity/cm-task-generation.service';
import { ActionType } from 'src/app/Shared/Enums/actionType';
import { MappedPOSMProduct } from 'src/app/Shared/Entity/Daily-Audit/mappedPOSMProduct';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';

@Component({
  selector: 'app-edit-audit-setup',
  templateUrl: './edit-audit-setup.component.html',
  styleUrls: ['./edit-audit-setup.component.css']
})
export class EditAuditSetupComponent implements OnInit {

  public form: FormGroup;
  auditSetup:AuditSetup;
  enumStatusTypes: MapObject[] = StatusTypes.statusType.slice(0,2);

  public canEditStartDate=false;
  public canEditEndDate = true;
  public selectedFromDate: NgbDateStruct;
  public selectedToDate: NgbDateStruct;
  public auditStatus: Number;
  public minDate:NgbDateStruct;

  public dailyTaskGenerate: DailyCMActivity = new DailyCMActivity();
  public distributionCheckProducts: MappedProduct[] = [];
  public facingCountProducts: MappedPOSMProduct[] = [];
  public planogramCheckProducts: MappedPOSMProduct[] = [];
  public priceAuditProducts: MappedProduct[] = [];
  public products: Product[] = [];
  public posmProducts: PosmProduct[] = [];


  public distCheckPtableSettings: IPTableSetting<colDef<keyof MappedProduct>> = {
    tableID: "dist-check-table",
    tableClass: "table table-border ",
    tableName: "Products",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "10%", internalName: "isSelected", disabilityInterName:"isDisabledCheck", sort: false, type: "checkbox", onClick:'true' },
      { headerName: "Product Code", width: "35%", internalName: "code", sort: true, type: "" },
      { headerName: "Product Name", width: "35%", internalName: "name", sort: true, type: "" },
      { headerName: "Is JTI Product", width: "20%", internalName: "isJTIProductLabel", sort: true, type: "" },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 5,
    enabledPagination: true,
    enabledCellClick: true
  };

  public facingCountPtableSettings: IPTableSetting<colDef<keyof MappedProduct>> = {
    tableID: "facing-count-table",
    tableClass: "table table-border ",
    tableName: "POSM Products",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "10%", internalName: "isSelected",disabilityInterName:"isDisabledCheck", sort: false, type: "checkbox", onClick:'true' },
      { headerName: "Product Code", width: "35%", internalName: "code", sort: true, type: "" },
      { headerName: "Product Name", width: "35%", internalName: "name", sort: true, type: "" },
      { headerName: "Is JTI Product", width: "20%", internalName: "isJTIProductLabel", sort: true, type: "" },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 5,
    enabledPagination: true,
    enabledCellClick: true
  };


  public planCheckPtableSettings: IPTableSetting<colDef<keyof MappedPOSMProduct>> = {
    tableID: "plan-check-table",
    tableClass: "table table-border ",
    tableName: "POSM Products",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "10%", internalName: "isSelected",disabilityInterName:"isDisabledCheck", sort: false, type: "checkbox", onClick:'true' },
      { headerName: "Product Code", width: "35%", internalName: "code", sort: true, type: "" },
      { headerName: "Product Name", width: "35%", internalName: "name", sort: true, type: "" },
      { headerName: "Is JTI Product", width: "20%", internalName: "isJTIProductLabel", sort: true, type: "" }
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 5,
    enabledPagination: true,
    enabledCellClick: true
  };

  public priceAuditPtableSettings: IPTableSetting<colDef<keyof MappedProduct>> = {
    tableID: "price-audit-table",
    tableClass: "table table-border ",
    tableName: "Products",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "10%", internalName: "isSelected",disabilityInterName:"isDisabledCheck", sort: false, type: "checkbox", onClick:'true' },
      { headerName: "Product Code", width: "35%", internalName: "code", sort: true, type: "" },
      { headerName: "Product Name", width: "35%", internalName: "name", sort: true, type: "" },
      { headerName: "Is JTI Product", width: "20%", internalName: "isJTIProductLabel", sort: true, type: "" },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 5,
    enabledPagination: true,
    enabledCellClick: true,
  };


  constructor(private router:Router,private route:ActivatedRoute,
    private alertService:AlertService,
    private commonService:CommonService,
    private cmTaskGenerationService: CmTaskGenerationService,
    private auditService:DailyAuditService,) { }

  ngOnInit() {
    this.minDate = this.commonService.dateToNgbDate(new Date());

    if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
      let auditSetupId = this.route.snapshot.params.id;
      this.auditService.getAuditSetupById(auditSetupId).subscribe(res=>{
        this.auditSetup = res;
        if(new Date() < new Date(this.auditSetup.fromDateStr) ) this.canEditStartDate = true;
        if (new Date() > new Date(this.auditSetup.toDateStr) ) this.canEditEndDate = false;
        this.selectedFromDate = this.commonService.dateToNgbDate(new Date(this.auditSetup.fromDateStr));
        this.selectedToDate = this.commonService.dateToNgbDate(new Date(this.auditSetup.toDateStr));
        this.auditStatus = this.auditSetup.status;
        this.fetchNecessaryData();
      });
    }
    else this.alertService.tosterDanger("Audit setup id not found");

  }

  fetchNecessaryData(){
    this.fnGetProducts();
    this.fnGetPOSMProducts();
  }

  public fnGetProducts(){
    this.alertService.fnLoading(true);
    this.cmTaskGenerationService.getProducts()
    .pipe(
      finalize(() => {
        this.alertService.fnLoading(false);
      })
    ).subscribe((res:APIResponsePage)=>{
      this.products=res.data.model||[];
      this.products.forEach(x => x.isJTIProductLabel = x.isJTIProduct ? 'YES' : 'NO');
      this.distributionCheckProducts = this.getWithoutRef(this.products);
      this.priceAuditProducts = this.getWithoutRef(this.products);
      this.setProductsInitialCheckState();
    });
  }

  setProductsInitialCheckState(){
    if(this.auditSetup.auditProducts){
      this.distributionCheckProducts.forEach(x=>{
        const product = this.auditSetup.auditProducts.find(p=>p.actionType === ActionType.DistributionCheckProduct && p.productId === x.id);
        if(product) x.isSelected = true;
        if(!this.canEditStartDate) x.isDisabledCheck = true;
      });
      this.priceAuditProducts.forEach(x=>{
        const product = this.auditSetup.auditProducts.find(p=>p.actionType === ActionType.PriceAuditProduct && p.productId === x.id);
        if(product) x.isSelected = true;
        if(!this.canEditStartDate) x.isDisabledCheck = true;
      });
    }
    
    if(!this.canEditStartDate){
      this.distributionCheckProducts = this.distributionCheckProducts.filter(x=>!!x.isSelected);
      this.priceAuditProducts = this.priceAuditProducts.filter(x=>!!x.isSelected);

      this.distributionCheckProducts.forEach(x=>x.isDisabledCheck = true);
      this.priceAuditProducts.forEach(x=>x.isDisabledCheck = true);
      this.facingCountProducts.forEach(x=>x.isDisabledCheck = true);
      this.planogramCheckProducts.forEach(x=>x.isDisabledCheck = true);
    }
    this.updateParentCheckboxForAudit();
    
  }



  
  getWithoutRef(value) {
    return JSON.parse(JSON.stringify(value));
  }
  
  public fnGetPOSMProducts(){
    this.alertService.fnLoading(true);
    this.cmTaskGenerationService.getPOSMProducts()
    .pipe(
      finalize(() => {
        this.alertService.fnLoading(false);
      })
    ).subscribe((res:APIResponsePage)=>{
      this.posmProducts=res.data.model||[];
      this.posmProducts.forEach(x => x.isJTIProductLabel = x.isJTIProduct ? 'YES' : 'NO');
      this.planogramCheckProducts = this.getWithoutRef(this.posmProducts.filter(pp => pp.isPlanogram));
      this.facingCountProducts = this.getWithoutRef(this.posmProducts);
      this.setPOSMInitialCheckState()
    });
  }
  
  setPOSMInitialCheckState(){
    if(this.auditSetup.auditPOSMProducts){
      this.planogramCheckProducts.forEach(x=>{
        const product = this.auditSetup.auditPOSMProducts.find(p=> p.actionType === ActionType.PlanogramCheckProduct && p.posmProductId === x.id);
        if(product) x.isSelected = true;
        if(!this.canEditStartDate) x.isDisabledCheck = true;

      });
      this.facingCountProducts.forEach(x=>{
        const product = this.auditSetup.auditPOSMProducts.find(p=> p.actionType === ActionType.FacingCountProduct && p.posmProductId === x.id);
        if(product) x.isSelected = true;
        if(!this.canEditStartDate) x.isDisabledCheck = true;

      });
      if(!this.canEditStartDate){
        this.planogramCheckProducts = this.planogramCheckProducts.filter(x=> !!x.isSelected);
        this.facingCountProducts = this.facingCountProducts.filter(x=> !!x.isSelected);
      }
      this.updateParentCheckboxForAudit();
    }
  }

  getAuditedProducts(){
    const selectedpriceAuditProducts = this.priceAuditProducts.filter(x=>x.isSelected).map(x=>{
      let auditProduct = this.auditSetup.auditProducts.find(ap=>ap.productId === x.id && ap.actionType === ActionType.PriceAuditProduct);
      if(!auditProduct){
         auditProduct = new AuditProduct();
         auditProduct.actionType = ActionType.PriceAuditProduct;
         auditProduct.productId = x.id;
         auditProduct.auditSetupId = this.auditSetup.id;
      }      
      return auditProduct;
    });
    const selectedDistributionCheckProducts = this.distributionCheckProducts.filter(x=>x.isSelected).map(x=>{
      let auditProduct = this.auditSetup.auditProducts.find(ap=>ap.productId === x.id && ap.actionType === ActionType.DistributionCheckProduct);
      if(!auditProduct){
        auditProduct = new AuditProduct();
        auditProduct.actionType = ActionType.DistributionCheckProduct;
        auditProduct.productId = x.id;
        auditProduct.auditSetupId = this.auditSetup.id;
      }      
      return auditProduct;
    });;    
    
    const auditProducts = [
      ...selectedpriceAuditProducts,
      ...selectedDistributionCheckProducts      
    ];
    return auditProducts;
  }

  getAuditedPosmProducts(){    
    const selectedfacingCountProducts = this.facingCountProducts.filter(x=>x.isSelected).map(x=>{
      let auditProduct = this.auditSetup.auditPOSMProducts.find(ap=>ap.posmProductId === x.id && ap.actionType === ActionType.FacingCountProduct);
      if(!auditProduct){
        auditProduct = new AuditPOSMProduct();
        auditProduct.actionType = ActionType.FacingCountProduct;
        auditProduct.posmProductId = x.id;
        auditProduct.auditSetupId = this.auditSetup.id
      }
      return auditProduct;
    });

    const selectedPlanogramCheckProducts = this.planogramCheckProducts.filter(x=>x.isSelected).map(x=>{
      let auditProduct=this.auditSetup.auditPOSMProducts.find(ap=>ap.posmProductId === x.id);
      if(!auditProduct){
        auditProduct = new AuditPOSMProduct();
        auditProduct.posmProductId = x.id;
        auditProduct.auditSetupId = this.auditSetup.id;
        auditProduct.actionType = ActionType.PlanogramCheckProduct;
      }      
      return auditProduct;
    });
    
    const auditProducts = [
      ...selectedfacingCountProducts,
      ...selectedPlanogramCheckProducts,
    ];
    return auditProducts;
  }

  isProductMappUpdated(checkedProducts: AuditProduct[]){        
    if(checkedProducts.length !== this.auditSetup.auditProducts.length) return true;
    if( checkedProducts.some(x=> !this.auditSetup.auditProducts.some(y=>y.actionType === x.actionType && y.productId === x.productId))) return true;
    return false;
  }

  isPOSMProductMappUpdated(checkedProducts: AuditPOSMProduct[]){        
    if(checkedProducts.length !== this.auditSetup.auditPOSMProducts.length) return true;
    if( checkedProducts.some(x=> !this.auditSetup.auditPOSMProducts.some(y=> y.actionType === x.actionType && y.posmProductId === x.posmProductId))) return true;
    return false;
  }

  submit(){
    let isUpated = false;
    let toDateObj = this.commonService.ngbDateToDate(this.selectedToDate);
    toDateObj.setHours(23,59,59);
    let toDate = toDateObj.toISOString();

    if(!moment(this.auditSetup.toDateStr).isSame(toDate,"second")) {
      isUpated = true;
      this.auditSetup.toDate = toDate;
    }
    if(this.canEditStartDate){
      let fromDateObj = this.commonService.ngbDateToDate(this.selectedFromDate);      
      let fromDate = fromDateObj.toISOString();
      if(!moment(this.auditSetup.fromDateStr).isSame(fromDate,"second")){
        isUpated = true;
        this.auditSetup.fromDate = fromDate;
      }      
      if(!this.auditSetup.auditProducts) this.auditSetup.auditProducts = [];
      const checkedProducts = this.getAuditedProducts();
      if(!isUpated) isUpated = this.isProductMappUpdated(checkedProducts);
      this.auditSetup.auditProducts = checkedProducts;
      const checkedPosmProducts = this.getAuditedPosmProducts();
      if(!isUpated) isUpated = this.isPOSMProductMappUpdated(checkedPosmProducts);
      this.auditSetup.auditPOSMProducts = checkedPosmProducts;      
    }

    if (this.auditStatus != this.auditSetup.status) {
      this.auditSetup.status = this.auditStatus
      isUpated = true;
    }
    
    if(!isUpated){
      this.alertService.tosterDanger("Nothing to update");
      return;
    }

    let update=()=>{
      this.auditService.editAuditSetup(this.auditSetup).subscribe(res=>{
        this.alertService.tosterSuccess("Successfully updated audit setup");
        this.router.navigate([RoutesLaout.AuditSetup,RoutesAudit.list]);
      });
    }

    if (this.auditSetup.status == 0) {
      this.alertService.confirm(`InActive setup can't be reverted to active. Are your sure to make the setup inactive?`,
        () => { update(); },
        () => {}
      );
      return;
    }

    this.auditService.getExistingAuditSetups({data:[this.auditSetup]}).subscribe(res=>{
      res = res.filter(x=>x.id != this.auditSetup.id);
      if(res.length) {
        if(this.alertService.loadingFlag) this.alertService.fnLoading(false);        

        this.alertService.confirm(`AuditSetup already exist in Salespoint ${this.auditSetup.salesPoint.name}. Do you want to stop the auditSetups before the new auditSetup starts?`, () => {
          update();
        },()=>{}); 

      }
      else update();      
    })
    
  }

  public fnDistCheckPtableCellClick(event) {
    if (event.cellName == "isSelected" && this.canEditStartDate) {
      let obj = this.distributionCheckProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForAudit();
    }
  }

  updateParentCheckboxForAudit() {
    let distributionCount = 0;
    let facingCount = 0;
    let planogramCount = 0;
    let priceAuditCount = 0;

    this.distributionCheckProducts.forEach((element) => {
      if (element.isSelected == true) {
        distributionCount++;
      }
    });

    if (distributionCount > 0) {
      this.dailyTaskGenerate.dailyAudit.isDistributionCheck = true;
    } else {
      this.dailyTaskGenerate.dailyAudit.isDistributionCheck = false;
    }

    this.facingCountProducts.forEach((element) => {
      if (element.isSelected == true) {
        facingCount++;
      }
    });

    if (facingCount > 0) {
      this.dailyTaskGenerate.dailyAudit.isFacingCount = true;
    } else {
      this.dailyTaskGenerate.dailyAudit.isFacingCount = false;
    }

    this.planogramCheckProducts.forEach((element) => {
      if (element.isSelected == true) {
        planogramCount++;
      }
    });

    if (planogramCount > 0) {
      this.dailyTaskGenerate.dailyAudit.isPlanogramCheck = true;
    } else {
      this.dailyTaskGenerate.dailyAudit.isPlanogramCheck = false;
    }


    this.priceAuditProducts.forEach((element) => {
      if (element.isSelected == true) {
        priceAuditCount++;
      }
    });

    if (priceAuditCount > 0) {
      this.dailyTaskGenerate.dailyAudit.isPriceAudit = true;
    } else {
      this.dailyTaskGenerate.dailyAudit.isPriceAudit = false;
    }




    if(priceAuditCount > 0 || planogramCount > 0 || facingCount> 0 || distributionCount > 0){
      this.dailyTaskGenerate.isAudit = true;
    }
    else{
      this.dailyTaskGenerate.isAudit = false;
    }
  }
  public fnFacingCountPtableCellClick(event) {
    if (event.cellName == "isSelected" && this.canEditStartDate) {
      let obj = this.facingCountProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForAudit();
    }
  }
  public fnPlanCheckPtableCellClick(event) {
    if (event.cellName == "isSelected" && this.canEditStartDate) {
      let obj = this.planogramCheckProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForAudit();
    }
  }

  public fnPriceAuditPtableCellClick(event) {
    if (event.cellName == "isSelected" && this.canEditStartDate) {
      let obj = this.priceAuditProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForAudit();
    }
  }

  handleBack(){
    this.router.navigate([RoutesAudit.parent,RoutesAudit.list]);
  }

}
