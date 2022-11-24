import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { finalize, first } from 'rxjs/operators';
import { NodeTree, SalesPoint, DailyCMActivity, Product, PosmProduct, APIResponsePage } from 'src/app/Shared/Entity';
import { TaskAssignedUserType, TaskAssignedUserTypeStrs } from 'src/app/Shared/Enums';
import { IDateRange } from 'src/app/Shared/interfaces';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { Utility } from 'src/app/Shared/utility';
import * as moment from 'moment';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { CmTaskGenerationService } from 'src/app/Shared/Services/DailyActivity/cm-task-generation.service';
import { AuditPOSMProduct, AuditProduct, AuditSetup } from 'src/app/Shared/Entity/Daily-Audit';
import { ActionType } from 'src/app/Shared/Enums/actionType';
import { DailyAuditService } from 'src/app/Shared/Services/DailyActivity/daily-audit.service';
import { RoutesAudit } from '../routesAudit';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';

@Component({
  selector: 'app-new-audit-setup',
  templateUrl: './new-audit-setup.component.html',
  styleUrls: ['./new-audit-setup.component.css']
})
export class NewAuditSetupComponent implements OnInit {

  public selectedFromDate: NgbDateStruct;
  public selectedToDate: NgbDateStruct;
  public minDate:NgbDateStruct;
  selectedUserType:TaskAssignedUserType= TaskAssignedUserType.BOTH;
  userTypes = TaskAssignedUserTypeStrs;
  public dailyTaskGenerate: DailyCMActivity = new DailyCMActivity();
  public distributionCheckProducts: Product[] = [];
  public facingCountProducts: PosmProduct[] = [];
  public planogramCheckProducts: PosmProduct[] = [];
  public priceAuditProducts: Product[] = [];
  public products: Product[] = [];
  public posmProducts: PosmProduct[] = [];

  nodeTree:NodeTree[]=[];

  salesPointIds:number[]=[]

  

  public distCheckPtableSettings: IPTableSetting = {
    tableID: "dist-check-table",
    tableClass: "table table-border ",
    tableName: "Products",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "10%", internalName: "isSelected", sort: false, type: "checkbox", onClick:'true' },
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

  public facingCountPtableSettings: IPTableSetting = {
    tableID: "facing-count-table",
    tableClass: "table table-border ",
    tableName: "POSM Products",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "10%", internalName: "isSelected", sort: false, type: "checkbox", onClick:'true' },
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


  public planCheckPtableSettings: IPTableSetting = {
    tableID: "plan-check-table",
    tableClass: "table table-border ",
    tableName: "POSM Products",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "10%", internalName: "isSelected", sort: false, type: "checkbox", onClick:'true' },
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

  public priceAuditPtableSettings: IPTableSetting = {
    tableID: "price-audit-table",
    tableClass: "table table-border ",
    tableName: "Products",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "10%", internalName: "isSelected", sort: false, type: "checkbox", onClick:'true' },
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


  constructor(private commonService:CommonService,
    private userService:UserService,
    private alertService:AlertService,
    private router: Router,
    private cmTaskGenerationService: CmTaskGenerationService,
    private auditService:DailyAuditService) { }

  ngOnInit() {
    this.minDate = this.commonService.dateToNgbDate(new Date());
    this.userService.getNodeTreeByCurrentUser().subscribe(data=>{
      this.nodeTree = data;
    })

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
    });
  }

  getWithoutRef(value) {
    return JSON.parse(JSON.stringify(value));
  }

  handleFromDateChange(){
    let from = this.commonService.ngbDateToDate(this.selectedFromDate).toISOString();
    let toDate = this.commonService.ngbDateToDate(this.selectedToDate).toISOString();
    if(moment(from).isAfter(toDate,"date") ) {
      this.selectedToDate = null;
    }
  }
  handleToDateChange(){    
    let from = this.commonService.ngbDateToDate(this.selectedFromDate).toISOString();
    let toDate = this.commonService.ngbDateToDate(this.selectedToDate).toISOString();
    if(moment(from).isAfter(toDate,"date") ) {
      this.selectedFromDate = null;
    }
  }
  getSelectedSalesPoints(){
    const salesPointIds:SalesPoint[]=[];
    let fun=(tree:NodeTree[])=>{
      tree.forEach(tr=>{
        if(!!tr.salesPoints && !!tr.salesPoints.length){
          tr.salesPoints.forEach(x=>{
            if(x.isSelected) salesPointIds.push(x);
          })
        }
        else if(!!tr.nodes) fun(tr.nodes);
      })
    }
    fun(this.nodeTree);
    return  salesPointIds;
  }

  getAuditableProducts(){
    const selectedpriceAuditProducts = this.priceAuditProducts.filter(x=>x.isSelected).map(x=>{
      const auditProduct = new AuditProduct();
      auditProduct.actionType = ActionType.PriceAuditProduct;
      auditProduct.productId = x.id;
      return auditProduct;
    });
    const selectedDistributionCheckProducts = this.distributionCheckProducts.filter(x=>x.isSelected).map(x=>{
      const auditProduct = new AuditProduct();
      auditProduct.actionType = ActionType.DistributionCheckProduct;
      auditProduct.productId = x.id;
      return auditProduct;
    });
    
    const auditProducts = [
      ...selectedpriceAuditProducts,
      ...selectedDistributionCheckProducts      
    ];
    return auditProducts;
  }

  getAuditablePOSMProducted(){        
    const selectedfacingCountProducts = this.facingCountProducts.filter(x=>x.isSelected).map(x=>{
      const auditProduct = new AuditPOSMProduct();
      auditProduct.actionType = ActionType.FacingCountProduct;
      auditProduct.posmProductId = x.id;
      return auditProduct;
    });

    const selectedPlanogramCheckProducts = this.planogramCheckProducts.filter(x=>x.isSelected).map(x=>{
      const auditProduct = new AuditPOSMProduct();
      auditProduct.posmProductId = x.id;
      auditProduct.actionType = ActionType.PlanogramCheckProduct;
      return auditProduct;
    });
    
    const auditProducts = [            
      ...selectedfacingCountProducts,
      ...selectedPlanogramCheckProducts,
    ];
    return auditProducts;
  }
  
  submit(){
    const setupList:AuditSetup[]=[];
    const fromDateStr = this.commonService.ngbDateToDate(this.selectedFromDate).toISOString();
    const toDateStr = this.commonService.ngbDateToDate(this.selectedToDate).toISOString();
    const dateRange:IDateRange={
      from:fromDateStr,
      to:toDateStr,
    };
    Utility.adjustDateRange(dateRange);
    const salesPoints = this.getSelectedSalesPoints();
    if(!salesPoints.length) {
      this.alertService.tosterDanger("No salespoint selected");
      return;
    }

    

    let createSetup=(sp:SalesPoint)=>{
      const setup = new AuditSetup();
      setup.code="ADT_"+sp.code;
      setup.fromDate=dateRange.from;
      setup.toDate = dateRange.to;
      setup.salesPointId = sp.salesPointId;
      setup.userType = this.selectedUserType;
      return setup;
    }

    const auditProducts = this.getAuditableProducts();
    const auditPOSMProducts = this.getAuditablePOSMProducted();

    if(auditPOSMProducts.length + auditProducts.length === 0) {
      this.alertService.tosterDanger("No products selected.");
      return;
    }

    salesPoints.forEach(sp=>{
        const setup = createSetup(sp);
        setup.auditProducts = auditProducts;
        setup.auditPOSMProducts = auditPOSMProducts;
        setupList.push(setup);
    });

    const createSetups=()=>{
      this.auditService.createNewAuditSetup({ data: setupList }).subscribe(res => {
        this.alertService.tosterSuccess("Audit setups created successfully");
        this.router.navigate([RoutesLaout.AuditSetup,RoutesAudit.list]);
      });
    }
    this.auditService.getExistingAuditSetups({data:setupList}).pipe(first()).subscribe(res=>{

      if(res.length) {
        if(this.alertService.loadingFlag) this.alertService.fnLoading(false);
        let sps = salesPoints.filter(x=> res.some(sv=>sv.salesPointId == x.salesPointId)).filter((sp,i,arr)=> arr.findIndex(x=>x.salesPointId == sp.salesPointId) === i);
        let spNames = sps.map(x=>x.name);

        this.alertService.confirm(`Audit setup already exist in Salespoint ${spNames.join(", ")}. Do you want to stop the audits before the new audits starts?`, () => {
          createSetups()
        },()=>{}); 

      }
      else createSetups();
    })

  }
  handleSalesPointSelect(event,salesPoint:SalesPoint){
    salesPoint.isSelected = event.target.checked;    
  }

  handleNodeSelect(item:NodeTree,checked:boolean){
    const node:NodeTree = this.getNodeById(item.node.id);
    if(!node) return;
    let fun = (trees:NodeTree[],checked:boolean)=>{
      trees.forEach(tr=>{
        tr.isSelected = checked;
        if(!!tr.nodes && tr.nodes.length) fun(tr.nodes,checked);
        else if(!!tr.salesPoints && !!tr.salesPoints.length){
          tr.salesPoints.forEach(sl=>{
            sl.isSelected = checked;
          })
        }
      })
    }
    fun([node],checked);
  }

  getNodeById(id:number){
    let find=(tree:NodeTree[])=>{
      let node = tree.find(t=>t.node.id === id);
      if(node) return node;
      //@ts-ignore
      else return find(tree.filter(x=>!!x.nodes).map(x=>x.nodes).flat());
    }
    return find(this.nodeTree);
  
  }

  public fnDistCheckPtableCellClick(event) {
    if (event.cellName == "isSelected") {
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
    if (event.cellName == "isSelected") {
      let obj = this.facingCountProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForAudit();
    }
  }

  public fnPlanCheckPtableCellClick(event) {
    if (event.cellName == "isSelected") {
      let obj = this.planogramCheckProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForAudit();
    }
  }

  public fnPriceAuditPtableCellClick(event) {
    if (event.cellName == "isSelected") {
      let obj = this.priceAuditProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForAudit();
    }
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
    });
  }

  handleBack(){
    this.router.navigate([RoutesAudit.parent,RoutesAudit.list]);
  }

}
