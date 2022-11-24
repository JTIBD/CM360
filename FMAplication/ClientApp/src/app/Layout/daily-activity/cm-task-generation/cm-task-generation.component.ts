import { Component, OnInit } from '@angular/core';
import { NgbDate, NgbCalendar,NgbDateStruct, NgbDateParserFormatter, NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CmTaskGenerationService } from 'src/app/Shared/Services/DailyActivity/cm-task-generation.service';
import { APIResponse, APIResponsePage } from 'src/app/Shared/Entity/Response';
import { PosmProduct, User, Route, DailyCMActivity, Outlet, UserInfo, SurveyQuestionSet, Product, POSMProduct, SalesPoint,  } from 'src/app/Shared/Entity';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { ActionType } from 'src/app/Shared/Enums/actionType';
import { finalize } from 'rxjs/operators';
import { ModalCmTaskDetailsComponent } from '../modal-cm-task-details/modal-cm-task-details.component';
import { POSMActionType } from 'src/app/Shared/Enums/posmActionType';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Channel } from 'src/app/Shared/Entity/Sales/channel';
import { SurveyService } from 'src/app/Shared/Services/Question-Details/survey.service';

@Component({
  selector: "app-cm-task-generation",
  templateUrl: "./cm-task-generation.component.html",
  styleUrls: ["./cm-task-generation.component.css"],
})
export class CmTaskGenerationComponent implements OnInit {
  public selectedDate: NgbDateStruct;
  // public selectedCompanies;
  // public companies: any[] = [];
  // public companiesNames = ['Uber', 'Microsoft', 'Flexigen'];

  public fmUsers: UserInfo[] = [];
  public routes: Route[] = [];
  public salesPoints: SalesPoint[] = [];
  public outlets: Outlet[] = [];
  public channels: Channel[] = [];
  public tasktypes: any;
  public posmProducts: PosmProduct[] = [];
  public posmInstallationProducts: PosmProduct[] = [];
  public posmRepairProducts: PosmProduct[] = [];
  public posmRemovalProducts: PosmProduct[] = [];
  public products: Product[] = [];
  public distributionCheckProducts: Product[] = [];
  public facingCountProducts: Product[] = [];
  public planogramCheckProducts: PosmProduct[] = [];
  public priceAuditProducts: Product[] = [];
  public cmUsers: User[] = [];
  public surveys: SurveyQuestionSet[] = [];
  public consumerSurveys: SurveyQuestionSet[] = [];
  public dailyTaskGenerate: DailyCMActivity = new DailyCMActivity();
  public selectedRoutes: any[] = [];
  public selectedSalesPoint: any;
  public selectedOutlets: any[] = [];
  public selectedCmUsers: any[] = [];
  public selectedCmTasks: any[] = [];
  public selectedTaskMappingOutlets: any[] = [];
  public dailyTaskGenerates: DailyCMActivity[] = [];
  public isEditDailyTaskGenrate: boolean = false;
  public outletCopyPasteMsg: string = '';
  public selectedTaskType: any;
  public selectedChannel: any;
  public disableOutlet: boolean;
  public show: boolean;
  public columnDefOfTskPtable: any [] = [];

  // hoveredDate: NgbDate | null = null;
  fromDate: NgbDate | null;
  // toDate: NgbDate | null;
  constructor(private cmTaskGenerationService: CmTaskGenerationService, 
      private calendar: NgbCalendar, 
      public formatter: NgbDateParserFormatter,
      private alertService: AlertService,
      private modalService: NgbModal,
      private surveyService: SurveyService,
      private router: Router) {
    this.fromDate = calendar.getToday();
    // this.toDate = calendar.getNext(calendar.getToday(), 'd', 10);
  }

  ngOnInit() {
    // this.companiesNames.forEach((c, i) => {
    //   this.companies.push({ id: i, name: c });
    // });
    this.fnGetFMUsers();
    this.fnGetPOSMProducts();
    this.fnGetProducts();
    this.fnGetSurvey();
    this.fnGetChannel();
    this.tasktypes = [{"id": "1", "name": "General"},{"id":2, "name":"Channel"}];
    this.selectedTaskType = this.tasktypes[0].id;
    this.show = true;
  }

  
  //#region get and change method
  public fnChangeFMUser(id: number) {
    // let salesPointId=+this.fmUsers.find(r=>r.id==id).salesPointId||0;
    // this.fnGetRouteBySalesPointId(salesPointId);
    // this.fnGetRouteByFMId(id);
    // this.fnGetCMUsersByFMId(id);
    this.alertService.fnLoading(true);
    forkJoin(
      //this.cmTaskGenerationService.getRoutesByFMId(id),
      this.cmTaskGenerationService.getCMUsersByFMId(id),
      this.cmTaskGenerationService.getSalesPointByFMId(id)
    )
    .pipe(
      finalize(() => {
        this.alertService.fnLoading(false);
      })
    ).subscribe((res:APIResponse[])=>{
        // route
        // this.routes=res[0].data||[];
        // console.log("routes : ", this.routes);
        // cm user
        this.cmUsers=res[0].data||[];
        console.log("cm users : ", this.cmUsers);
        //Sales Point
        this.salesPoints=res[1].data||[];
        console.log("sales point : ", this.salesPoints);
    });
  }

  public fnChangeRoutes(event) {
    this.fnGetOutletsByRoutes();
  }
  public fnChangeSalespoint(id:number) {
    this.selectedChannel = [];
    this.selectedCmTasks = [];
    this.fnGetRouteBySalesPointId(id);
  }
public fnChangeTaskType(id:number) {
  this.outlets = [];  
  this.selectedOutlets = [];  
  this.selectedCmTasks = [];
  this.fnGetColumnDef();
   if(id == 2){
    this.disableOutlet =true;
    this.show = false;  
   }else{
     this.disableOutlet = false;
     this.show = true;    
   }
}
public fnChangeChannel(id:number) {  
  if(this.selectedSalesPoint == null || this.selectedSalesPoint == undefined)
  {
    return;
  }
  this.selectedCmTasks = [];
  this.alertService.fnLoading(true);
  this.cmTaskGenerationService.getOutletsByChannelId(id, this.selectedSalesPoint)
  .pipe(
    finalize(() => {
      this.alertService.fnLoading(false);
    })
  ).subscribe((res:APIResponse)=>{
    this.outlets = res.data || [];
      this.selectedOutlets=res.data.map(o=> o.outletId)||[];
      console.log("outletschan : ", this.selectedOutlets);
    });
}
  fnGetFMUsers(){
    this.alertService.fnLoading(true);
    this.cmTaskGenerationService.getFMUsers()
    .pipe(
      finalize(() => {
        this.alertService.fnLoading(false);
      })
    ).subscribe((res:APIResponse)=>{
        this.fmUsers=res.data||[];
        console.log("FM users: ", this.fmUsers);
    });
  }

  fnGetChannel(){
    this.alertService.fnLoading(true);
    this.cmTaskGenerationService.getChannels()
    .pipe(
      finalize(() => {
        this.alertService.fnLoading(false);
      })
    ).subscribe((res:APIResponse)=>{
        this.channels=res.data||[];
        console.log("Channels: ", this.channels);
    });
  }

  

  // public fnGetRouteBySalesPointId(salesPointId:number){
  //   this.cmTaskGenerationService.getRoutesBySalesPointId(salesPointId).subscribe((res:APIResponse)=>{
  //       this.routes=res.data||[];
  //       console.log("routes : ", this.routes);
  //   });
  // }
  
  public fnGetRouteByFMId(salesPointId:number){
    this.alertService.fnLoading(true);
    this.cmTaskGenerationService.getRoutesByFMId(salesPointId)
    .pipe(
      finalize(() => {
        this.alertService.fnLoading(false);
      })
    ).subscribe((res:APIResponse)=>{
        this.routes=res.data||[];
        console.log("routes : ", this.routes);
      });
  }

  public fnGetRouteBySalesPointId(salesPointId:number){
    this.alertService.fnLoading(true);
    this.cmTaskGenerationService.getRoutesBySalesPointId(salesPointId)
    .pipe(
      finalize(() => {
        this.alertService.fnLoading(false);
      })
    ).subscribe((res:APIResponse)=>{
      this.routes.push(...(res.data||[]));
        console.log("routes : ", this.routes);
      });

    // if (this.selectedSalesPoint.length == 0) {
    //   this.routes = [];
    //   this.selectedRoutes = [];
    //   return;
    // }

    // // remove outlet if not select related route
    // const filterOut = this.routes.filter((o) =>
    //   this.selectedSalesPoint.find((sr) => sr == o.salesPointId)
    // );
    // this.routes = filterOut;
    // // this.selectedOutlets = this.outlets.map((r) => r.outletId);
    // this.selectedRoutes = this.selectedRoutes.filter(sr => this.routes.find(o => o.routeId == sr));

    // // add new outlet
    // this.selectedSalesPoint.forEach(rId => {
    //   if(!this.routes.find(ot => ot.salesPointId==rId)) {
    //     this.alertService.fnLoading(true);
    //     this.cmTaskGenerationService.getRoutesBySalesPointId(rId)
    //     .pipe(
    //       finalize(() => {
    //         this.alertService.fnLoading(false);
    //       })
    //     ).subscribe((res:APIResponse)=>{
    //         this.routes.push(...(res.data||[]));
    //         // this.selectedOutlets.push(...(this.outlets.map(r=>r.outletId)));
    //         // this.selectedOutlets= this.selectedOutlets.concat(...(this.outlets.map(r=>r.outletId)));
    //         // this.selectedOutlets = this.outlets.map((r) => r.outletId);
    //         // console.log("routed.....@@@ : ", this.routes);
    //         // console.log("selected salespoint : ", this.selectedSalesPoint);
    //       });
    //   }
    // });
  }



  public fnGetOutletsByRoutes() {
    if (this.selectedRoutes.length == 0) {
      this.outlets = [];
      this.selectedOutlets = [];
      return;
    }

    // remove outlet if not select related route
    const filterOut = this.outlets.filter((o) =>
      this.selectedRoutes.find((sr) => sr == o.routeId)
    );
    this.outlets = filterOut;
    // this.selectedOutlets = this.outlets.map((r) => r.outletId);
    this.selectedOutlets = this.selectedOutlets.filter(sr => this.outlets.find(o => o.outletId == sr));

    // add new outlet
    this.selectedRoutes.forEach(rId => {
      if(!this.outlets.find(ot => ot.routeId==rId)) {
        this.alertService.fnLoading(true);
        this.cmTaskGenerationService.getOutletsByRouteId(rId)
        .pipe(
          finalize(() => {
            this.alertService.fnLoading(false);
          })
        ).subscribe((res:APIResponse)=>{
            this.outlets.push(...(res.data||[]));
            // this.selectedOutlets.push(...(this.outlets.map(r=>r.outletId)));
            // this.selectedOutlets= this.selectedOutlets.concat(...(this.outlets.map(r=>r.outletId)));
            // this.selectedOutlets = this.outlets.map((r) => r.outletId);
            console.log("outlets : ", this.outlets);
            console.log("selected outlets : ", this.selectedOutlets);
          });
      }
    });
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
      this,this.posmProducts.forEach(x => x.isDigitalSignatureEnableLabel = x.isDigitalSignatureEnable ? 'YES' : 'NO');
      this.posmInstallationProducts=this.getWithoutRef(this.posmProducts);
      this.posmRepairProducts=this.getWithoutRef(this.posmProducts);
      this.posmRemovalProducts=this.getWithoutRef(this.posmProducts);
      this.planogramCheckProducts = this.getWithoutRef(this.posmProducts.filter(pp => pp.isPlanogram));
      console.log("this.posmProduct,", this.posmProducts);
    });
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
      this.facingCountProducts = this.getWithoutRef(this.products);
      this.priceAuditProducts = this.getWithoutRef(this.products);
      console.log("this.products ,", this.products);
    });
  }

  fnGetCMUsersByFMId(fmId:number){
    this.alertService.fnLoading(true);
    this.cmTaskGenerationService.getCMUsersByFMId(fmId)
    .pipe(
      finalize(() => {
        this.alertService.fnLoading(false);
      })
    ).subscribe((res:APIResponse)=>{
        this.cmUsers=res.data||[];
    });
  }

  fnGetSurvey(){
    this.alertService.fnLoading(true);
    this.surveyService.getAllQuestionSet()
    .pipe(
      finalize(() => {
        this.alertService.fnLoading(false);
      })
    ).subscribe((res:APIResponse)=>{
      console.log("survey: ", res);
      this.surveys = res.data || [];
      this.consumerSurveys = this.getWithoutRef(this.surveys);
    });
  }

  // onDateSelection(date: NgbDate) {
  //   if (!this.fromDate && !this.toDate) {
  //     this.fromDate = date;
  //   } else if (this.fromDate && !this.toDate && date && date.after(this.fromDate)) {
  //     this.toDate = date;
  //   } else {
  //     this.toDate = null;
  //     this.fromDate = date;
  //   }
  // }

  fnChangeOutlets(event: any) {
    console.log(this.selectedOutlets);
  }

  fnChangeCmUsers(event: any) {
    console.log(this.selectedCmUsers);
  }

  fnChangeTaskMappingOutlets(event: any) {
    console.log(this.selectedTaskMappingOutlets);
  }
  
  fnCopyPasteOutlets(event: any) {
    event.stopPropagation();
    event.preventDefault();
    let items = event.clipboardData.getData('Text').split(' ');
    console.log('copy paste item - ', items);
    let selectOutletIds = this.outlets.filter(o => !!items.find(i => i == o.code) && 
                                                    !(!!this.selectedOutlets.find(i => i == o.outletId)))
                                        .map(m => m.outletId);
    // let data = 
    this.selectedOutlets = [...this.selectedOutlets,...selectOutletIds];
    console.log(this.selectedOutlets);
    let foundOutletCount = this.outlets.filter(o => !!items.find(i => i == o.code)).length;
    this.outletCopyPasteMsg = `${foundOutletCount} outlet found and ${items.length - foundOutletCount} outlet not found by copy paste 'code'`;
  }
  //#endregion

  //#region cm Task creation

  fnClickNewCmTask() {
    console.log("selected cm users", this.selectedCmUsers);
    console.log("selected outlets", this.selectedOutlets);
    console.log("selected date", this.selectedDate);
    let hasError = false;
    let errorMsg = "";

    if (!this.dailyTaskGenerate.assignedFMUserId) {
      hasError = true;
      errorMsg = "Please select fm user";
    } 
    // else if (!this.selectedRoutes || this.selectedRoutes.length == 0) {
    //   hasError = true;
    //   errorMsg = "Please select at least one route";
    // } 
    else if (!this.selectedOutlets || this.selectedOutlets.length == 0) {
      hasError = true;
      errorMsg = "Please select at least one outlet";
    } else if (!this.selectedCmUsers || this.selectedCmUsers.length == 0) {
      hasError = true;
      errorMsg = "Please select at least one cm user";
    } else if (!this.selectedDate || !this.validateDate(this.selectedDate)) {
      hasError = true;
      errorMsg = "Please select valid date";
    }

    if (hasError) {
      this.alertService.tosterDanger(errorMsg);
      return;
    }

    this.selectedCmUsers.forEach((cmu) => {
      this.selectedOutlets.forEach((out) => {
        const fmUser = this.fmUsers.find(
          (f) => f.id == this.dailyTaskGenerate.assignedFMUserId
        );
        const outlet = this.outlets.find((o) => o.outletId == out);
        let route = this.routes.find((r) => r.routeId == outlet.routeId);
        const cmUser = this.cmUsers.find((c) => c.id == cmu);
        let channel = this.channels.find((c) => c.channelID == this.selectedChannel);
        if(route === undefined)
        {
          route = new Route();
          route.routeId = 0;
          route.routeName = '';
        }
        if(channel == undefined){
          channel = new Channel();
          channel.name = ''
        }
        const cmTask = {
          fmUserId: fmUser.id,
          fmUserName: fmUser.name,
          outletId: outlet.outletId,
          outletName: outlet.name + " (" + outlet.code + ")",
          routeId: route.routeId,
          routeName: route.routeName,
          channelName: channel == undefined ? "": channel.name,
          cmUserId: cmUser.id,
          cmUserName: cmUser.name,
          id:
            fmUser.id.toString() +
            route.routeId.toString() +
            outlet.outletId.toString() +
            cmUser.id.toString(),
          displayDate: this.dateToString(this.selectedDate),
          date: new Date(
            this.selectedDate.year,
            this.selectedDate.month - 1,
            this.selectedDate.day
          ),
        };

        const cmtObj = this.selectedCmTasks.find(
          (c) =>
            c.fmUserId == fmUser.id &&
            c.routeId ==  route.routeId &&
            c.outletId == outlet.outletId &&
            c.cmUserId == cmUser.id
        );
        if (!cmtObj) {
          this.selectedCmTasks.push(cmTask);
        }
      });
    });
    console.log("selected cm tasks", this.selectedCmTasks);
  }

  public fnGetColumnDef(): any[]
  {
    if(this.selectedTaskType == 2){
      this.cmTaskPtableSettings.tableColDef = [
        { 
          headerName: "Date", 
          width: "20%", 
          internalName: "displayDate", 
          sort: true, 
          type: "",
         },
        { 
          headerName: "FM User ", 
          width: "20%", 
          internalName: "fmUserName", 
          sort: true, 
          type: "",
         },
        { 
          headerName: "Channel", 
          width: "20%", 
          internalName: "channelName", 
          sort: true, 
          type: "",
         },
        { 
          headerName: "Outlet", 
          width: "20%", 
          internalName: "outletName", 
          sort: true, 
          type: "",
         },
        { 
          headerName: "CM User ", 
          width: "20%", 
          internalName: "cmUserName", 
          sort: true, 
          type: "",
         },
      ];
      
    }
    else {
      this.cmTaskPtableSettings.tableColDef = [
        { 
          headerName: "Date", 
          width: "20%", 
          internalName: "displayDate", 
          sort: true, 
          type: "",
         },
        { 
          headerName: "FM User ", 
          width: "20%", 
          internalName: "fmUserName", 
          sort: true, 
          type: "",
         },
        { 
          headerName: "Route", 
          width: "20%", 
          internalName: "routeName", 
          sort: true, 
          type: "",
         },
        { 
          headerName: "Outlet", 
          width: "20%", 
          internalName: "outletName", 
          sort: true, 
          type: "",
         },
        { 
          headerName: "CM User ", 
          width: "20%", 
          internalName: "cmUserName", 
          sort: true, 
          type: "",
         },
      ];
    }
      
  
  return this.columnDefOfTskPtable;
  }
  public cmTaskPtableSettings: IPTableSetting = {
    tableID: "cm-task-table",
    tableClass: "table table-border ",
    tableName: "Task For CM",
    tableRowIDInternalName: "id",
    enabledCheckbox: true,
    checkboxCallbackFn: true,
    tableColDef: [
      { 
        headerName: "Date", 
        width: "20%", 
        internalName: "displayDate", 
        sort: true, 
        type: "",
       },
      { 
        headerName: "FM User ", 
        width: "20%", 
        internalName: "fmUserName", 
        sort: true, 
        type: "",
       },
      { 
        headerName: "Route", 
        width: "20%", 
        internalName: "routeName", 
        sort: true, 
        type: "",
       },
      { 
        headerName: "Outlet", 
        width: "20%", 
        internalName: "outletName", 
        sort: true, 
        type: "",
       },
      { 
        headerName: "CM User ", 
        width: "20%", 
        internalName: "cmUserName", 
        sort: true, 
        type: "",
       },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 10,
    enabledPagination: true,
    //enabledAutoScrolled:true,
    //enabledCellClick: true,
    enabledColumnFilter: true,
    enabledDeleteBtn: true,
  };

  public fnCmTaskPtableCellClick(event) {
    console.log("cm task cell click: ", event);

    if (event.cellName == "check-box") {
      this.cmTaskSelectForMapping(event.record);
    }
  }

  public fnCmTaskCustomTrigger(event) {
    console.log("cm task custom  click: ", event);

    if (event.action == "delete-item") {
      this.cmTaskDelete(event.record);
    }
  }

  fnCmTaskPtableCheckboxClick(event) {
    console.log("cm task check click: ", event);
  }

  cmTaskSelectForMapping(objs) {
    this.selectedTaskMappingOutlets = objs.map((out) => out.id);
  }

  cmTaskDelete(obj) {
    const index: number = this.selectedCmTasks.indexOf(obj);
    if (index !== -1) {
      this.selectedCmTasks.splice(index, 1);
    }
  }
  //#endregion

  //#region cm task generated

  fnClickGenerateCmTask() {
    if (this.isEditDailyTaskGenrate) {
      this.updateDailyCmTask();
      this.clearAllSelectInTaskMappingBlock();
      return;
    }
    // console.log('selected task mapping outlets', this.selectedTaskMappingOutlets);
    let hasError = false;
    let errorMsg = "";

    if (
      !this.selectedTaskMappingOutlets ||
      this.selectedTaskMappingOutlets.length == 0
    ) {
      hasError = true;
      errorMsg = "Please select at least one outlet mapping";
    }

    if (hasError) {
      this.alertService.tosterDanger(errorMsg);
      return;
    }

    this.selectedTaskMappingOutlets.forEach((tmoId) => {
      const filterCmTasks = this.selectedCmTasks.filter((c) => c.id == tmoId);
      // console.log('filter generated cm task', filterCmTasks);
      filterCmTasks.forEach((cmtObj) => {
        let dailyCmAct = new DailyCMActivity();
        dailyCmAct.id = cmtObj.id;
        dailyCmAct.assignedFMUserId = cmtObj.fmUserId;
        dailyCmAct.outletId = cmtObj.outletId;
        dailyCmAct.cmId = cmtObj.cmUserId;
        dailyCmAct.date = cmtObj.date;
        dailyCmAct.isPOSM = this.dailyTaskGenerate.isPOSM;
        dailyCmAct.isAudit = this.dailyTaskGenerate.isAudit;
        dailyCmAct.isSurvey = this.dailyTaskGenerate.isSurvey;
        dailyCmAct.isConsumerSurveyActive = this.dailyTaskGenerate.isConsumerSurveyActive;

        //#region POSM
        dailyCmAct.dailyPOSM.isPOSMInstallation = this.dailyTaskGenerate.dailyPOSM.isPOSMInstallation;
        dailyCmAct.dailyPOSM.isPOSMRepair = this.dailyTaskGenerate.dailyPOSM.isPOSMRepair;
        dailyCmAct.dailyPOSM.isPOSMRemoval = this.dailyTaskGenerate.dailyPOSM.isPOSMRemoval;

        dailyCmAct.dailyPOSM.posmInstallationProducts = this.posmInstallationProducts
          .filter((p) => p.isSelected)
          .map((p) => {
            const posp = new POSMProduct();
            posp.productId = p.id;
            posp.actionType = POSMActionType.Installation;
            posp.productType = p.type;
            return posp;
          });

        dailyCmAct.dailyPOSM.posmRepairProducts = this.posmRepairProducts
          .filter((p) => p.isSelected)
          .map((p) => {
            const posp = new POSMProduct();
            posp.productId = p.id;
            posp.actionType = POSMActionType.Repair;
            posp.productType = p.type;
            return posp;
          });

        dailyCmAct.dailyPOSM.posmRemovalProducts = this.posmRemovalProducts
          .filter((p) => p.isSelected)
          .map((p) => {
            const posp = new POSMProduct();
            posp.productId = p.id;
            posp.actionType = POSMActionType.Removal;
            posp.productType = p.type;
            return posp;
          });
        //#endregion

        //#region Audit
        dailyCmAct.dailyAudit.isDistributionCheck = this.dailyTaskGenerate.dailyAudit.isDistributionCheck;
        dailyCmAct.dailyAudit.isFacingCount = this.dailyTaskGenerate.dailyAudit.isFacingCount;
        dailyCmAct.dailyAudit.isPlanogramCheck = this.dailyTaskGenerate.dailyAudit.isPlanogramCheck;
        dailyCmAct.dailyAudit.isPriceAudit = this.dailyTaskGenerate.dailyAudit.isPriceAudit;

        dailyCmAct.dailyAudit.distributionCheckProducts = this.distributionCheckProducts
          .filter((p) => p.isSelected)
          .map((p) => {
            const pro = { productId: p.id, actionType: ActionType.DistributionCheckProduct };
            return pro;
          });

        dailyCmAct.dailyAudit.facingCountProducts = this.facingCountProducts
          .filter((p) => p.isSelected)
          .map((p) => {
            const pro = { productId: p.id, actionType: ActionType.FacingCountProduct };
            return pro;
          });

        dailyCmAct.dailyAudit.planogramCheckProducts = this.planogramCheckProducts
          .filter((p) => p.isSelected)
          .map((p) => {
            const pro = { 
              pOSMProductId: p.id, 
              actionType: ActionType.PlanogramCheckProduct, 
              productType: p.type, 
            };
            return pro;
          });

        dailyCmAct.dailyAudit.priceAuditProducts = this.priceAuditProducts
          .filter((p) => p.isSelected)
          .map((p) => {
            const pro = { productId: p.id, actionType: ActionType.PriceAuditProduct };
            return pro;
          });
        //#endregion

        //#region Survey
        dailyCmAct.surveys = this.surveys
          .filter((p) => p.isSelected)
          .map((p) => {
            const sur = { id: p.id, isConsumerSurvey: false };
            return sur;
          });

        dailyCmAct.consumerSurveys = this.consumerSurveys
          .filter((p) => p.isSelected)
          .map((p) => {
            const sur = { id: p.id, isConsumerSurvey: true };
            return sur;
          });
        //#endregion

        //#region for display
        dailyCmAct.fmUserName = cmtObj.fmUserName;
        dailyCmAct.routeName = cmtObj.routeName;
        dailyCmAct.outletName = cmtObj.outletName;
        dailyCmAct.cmUserName = cmtObj.cmUserName;
        dailyCmAct.displayDate = cmtObj.displayDate;
        dailyCmAct.displayIsPOSM = this.dailyTaskGenerate.isPOSM ? "YES" : "NO";
        dailyCmAct.displayIsAudit = this.dailyTaskGenerate.isAudit
          ? "YES"
          : "NO";
        dailyCmAct.displayIsSurvey = this.dailyTaskGenerate.isSurvey
          ? "YES"
          : "NO";
        dailyCmAct.displayIsConsSurAct = this.dailyTaskGenerate
          .isConsumerSurveyActive
          ? "YES"
          : "NO";
        dailyCmAct.displayDetails = "More..";
        //#endregion

        this.dailyTaskGenerates.push(dailyCmAct);

        // remove from cm task after generate task
        this.cmTaskDelete(cmtObj);
      });
    });

    this.clearAllSelectInTaskMappingBlock();
  }

  updateDailyCmTask() {
    let dailyCmAct = this.dailyTaskGenerates.find(
      (dt) => dt.id == this.dailyTaskGenerate.id
    );
    dailyCmAct.isPOSM = this.dailyTaskGenerate.isPOSM;
    dailyCmAct.isAudit = this.dailyTaskGenerate.isAudit;
    dailyCmAct.isSurvey = this.dailyTaskGenerate.isSurvey;
    dailyCmAct.isConsumerSurveyActive = this.dailyTaskGenerate.isConsumerSurveyActive;

    //#region POSM
    dailyCmAct.dailyPOSM.isPOSMInstallation = this.dailyTaskGenerate.dailyPOSM.isPOSMInstallation;
    dailyCmAct.dailyPOSM.isPOSMRepair = this.dailyTaskGenerate.dailyPOSM.isPOSMRepair;
    dailyCmAct.dailyPOSM.isPOSMRemoval = this.dailyTaskGenerate.dailyPOSM.isPOSMRemoval;

    dailyCmAct.dailyPOSM.posmInstallationProducts = this.posmInstallationProducts
      .filter((p) => p.isSelected)
      .map((p) => {
        const posp = new POSMProduct();
        posp.productId = p.id;
        posp.actionType = POSMActionType.Installation;
        posp.productType = p.type;
        return posp;
      });

    dailyCmAct.dailyPOSM.posmRepairProducts = this.posmRepairProducts
      .filter((p) => p.isSelected)
      .map((p) => {
        const posp = new POSMProduct();
        posp.productId = p.id;
        posp.actionType = POSMActionType.Repair;
        posp.productType = p.type;
        return posp;
      });

    dailyCmAct.dailyPOSM.posmRemovalProducts = this.posmRemovalProducts
      .filter((p) => p.isSelected)
      .map((p) => {
        const posp = new POSMProduct();
        posp.productId = p.id;
        posp.actionType = POSMActionType.Removal;
        posp.productType = p.type;
        return posp;
      });
    //#endregion

    //#region Audit
    dailyCmAct.dailyAudit.isDistributionCheck = this.dailyTaskGenerate.dailyAudit.isDistributionCheck;
    dailyCmAct.dailyAudit.isFacingCount = this.dailyTaskGenerate.dailyAudit.isFacingCount;
    dailyCmAct.dailyAudit.isPlanogramCheck = this.dailyTaskGenerate.dailyAudit.isPlanogramCheck;
    dailyCmAct.dailyAudit.isPriceAudit = this.dailyTaskGenerate.dailyAudit.isPriceAudit;

    dailyCmAct.dailyAudit.distributionCheckProducts = this.distributionCheckProducts
      .filter((p) => p.isSelected)
      .map((p) => {
        const pro = { productId: p.id, actionType: ActionType.DistributionCheckProduct };
        return pro;
      });

    dailyCmAct.dailyAudit.facingCountProducts = this.facingCountProducts
      .filter((p) => p.isSelected)
      .map((p) => {
        const pro = { productId: p.id, actionType: ActionType.FacingCountProduct };
        return pro;
      });

    dailyCmAct.dailyAudit.planogramCheckProducts = this.planogramCheckProducts
      .filter((p) => p.isSelected)
      .map((p) => {
        const pro = {
          pOSMProductId: p.id,
          actionType: ActionType.PlanogramCheckProduct,
          productType: p.type, 
        };
        return pro;
      });

    dailyCmAct.dailyAudit.priceAuditProducts = this.priceAuditProducts
      .filter((p) => p.isSelected)
      .map((p) => {
        const pro = { productId: p.id, actionType: ActionType.PriceAuditProduct };
        return pro;
      });
    //#endregion

    //#region Survey
    dailyCmAct.surveys = this.surveys
      .filter((p) => p.isSelected)
      .map((p) => {
        const sur = { id: p.id, isConsumerSurvey: false };
        return sur;
      });

    dailyCmAct.consumerSurveys = this.consumerSurveys
      .filter((p) => p.isSelected)
      .map((p) => {
        const sur = { id: p.id, isConsumerSurvey: true };
        return sur;
      });
    //#endregion

    //#region for display
    dailyCmAct.displayIsPOSM = this.dailyTaskGenerate.isPOSM ? "YES" : "NO";
    dailyCmAct.displayIsAudit = this.dailyTaskGenerate.isAudit ? "YES" : "NO";
    dailyCmAct.displayIsSurvey = this.dailyTaskGenerate.isSurvey ? "YES" : "NO";
    dailyCmAct.displayIsConsSurAct = this.dailyTaskGenerate
      .isConsumerSurveyActive
      ? "YES"
      : "NO";
    //#endregion
  }

  fnClickClearCmTask() {
    this.clearAllSelectInTaskMappingBlock();
  }

  clearAllSelectInTaskMappingBlock() {
    this.isEditDailyTaskGenrate = false;
    this.dailyTaskGenerate.id = undefined;
    this.dailyTaskGenerate.outletName = "";
    this.dailyTaskGenerate.cmUserName = "";

    this.selectedTaskMappingOutlets = [];
    this.dailyTaskGenerate.isPOSM = false;
    this.dailyTaskGenerate.dailyPOSM.isPOSMInstallation = false;
    this.dailyTaskGenerate.dailyPOSM.isPOSMRepair = false;
    this.dailyTaskGenerate.dailyPOSM.isPOSMRemoval = false;
    this.posmInstallationProducts.forEach((pm) => (pm.isSelected = false));
    this.posmRepairProducts.forEach((pm) => (pm.isSelected = false));
    this.posmRemovalProducts.forEach((pm) => (pm.isSelected = false));

    this.dailyTaskGenerate.isAudit = false;
    this.dailyTaskGenerate.dailyAudit.isDistributionCheck = false;
    this.distributionCheckProducts.forEach((pm) => (pm.isSelected = false));
    this.dailyTaskGenerate.dailyAudit.isFacingCount = false;
    this.facingCountProducts.forEach((pm) => (pm.isSelected = false));
    this.dailyTaskGenerate.dailyAudit.isPlanogramCheck = false;
    this.planogramCheckProducts.forEach((pm) => (pm.isSelected = false));
    this.dailyTaskGenerate.dailyAudit.isPriceAudit = false;
    this.priceAuditProducts.forEach((pm) => (pm.isSelected = false));

    this.dailyTaskGenerate.isSurvey = false;
    this.surveys.forEach((sur) => (sur.isSelected = false));
    this.dailyTaskGenerate.isConsumerSurveyActive = false;
    this.consumerSurveys.forEach((sur) => (sur.isSelected = false));
  }

  public generatedCmTaskPtableSettings: IPTableSetting = {
    tableID: "generated-cm-task-table",
    tableClass: "table table-border ",
    tableName: "Generated Task For CM",
    tableRowIDInternalName: "id",
    tableColDef: [
      {
        headerName: "Date",
        width: "8%",
        internalName: "displayDate",
        sort: true,
        type: "",
      },
      {
        headerName: "FM User ",
        width: "12%",
        internalName: "fmUserName",
        sort: true,
        type: "",
      },
      {
        headerName: "Route",
        width: "12%",
        internalName: "routeName",
        sort: true,
        type: "",
      },
      {
        headerName: "Outlet",
        width: "12%",
        internalName: "outletName",
        sort: true,
        type: "",
      },
      {
        headerName: "CM User ",
        width: "12%",
        internalName: "cmUserName",
        sort: true,
        type: "",
      },
      {
        headerName: "Is POSM",
        width: "7%",
        internalName: "displayIsPOSM",
        sort: true,
        type: "",
      },
      {
        headerName: "Is Audit",
        width: "7%",
        internalName: "displayIsAudit",
        sort: true,
        type: "",
      },
      {
        headerName: "Is Survey",
        width: "7%",
        internalName: "displayIsSurvey",
        sort: true,
        type: "",
      },
      {
        headerName: "Is Consumer Survey",
        width: "10%",
        internalName: "displayIsConsSurAct",
        sort: true,
        type: "",
      },
      {
        headerName: "Details",
        width: "8%",
        internalName: "displayDetails",
        sort: true,
        type: "button",
        onClick: "true",
        innerBtnIcon: "fa fa-copy",
      },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 10,
    enabledPagination: true,
    //enabledAutoScrolled:true,
    enabledCellClick: true,
    enabledColumnFilter: true,
    enabledEditBtn: true,
    enabledDeleteBtn: true,
  };

  public fnGeneratedCmTaskPtableCellClick(event) {
    console.log("cm task cell click: ", event);
    if (event.cellName == "displayDetails") {
      this.openCmTaskDetailsModal(event.record);
    }
  }

  public fnGeneratedCmTaskCustomTrigger(event) {
    console.log("cm task custom  click: ", event);

    if (event.action == "delete-item") {
      this.generatedCmTaskDelete(event.record);
    } else if (event.action == "edit-item") {
      this.generatedCmTaskEdit(event.record);
    }
  }

  generatedCmTaskDelete(obj) {
    this.alertService.confirm(
      "Are you sure want to remove from list?",
      () => {
        const index: number = this.dailyTaskGenerates.indexOf(obj);
        if (index !== -1) {
          this.dailyTaskGenerates.splice(index, 1);
        }
      },
      () => {}
    );
  }

  generatedCmTaskEdit(obj) {
    const dailyCmAct = this.dailyTaskGenerates.find((dt) => dt.id == obj.id);
    console.log("edit generated cm task: ", obj);
    this.isEditDailyTaskGenrate = true;
    this.dailyTaskGenerate.id = dailyCmAct.id;
    this.dailyTaskGenerate.outletName = dailyCmAct.outletName;
    this.dailyTaskGenerate.cmUserName = dailyCmAct.cmUserName;

    this.dailyTaskGenerate.isPOSM = dailyCmAct.isPOSM;
    this.dailyTaskGenerate.isAudit = dailyCmAct.isAudit;
    this.dailyTaskGenerate.isSurvey = dailyCmAct.isSurvey;
    this.dailyTaskGenerate.isConsumerSurveyActive =
      dailyCmAct.isConsumerSurveyActive;

    //#region POSM
    this.dailyTaskGenerate.dailyPOSM.isPOSMInstallation =
      dailyCmAct.dailyPOSM.isPOSMInstallation;
    this.dailyTaskGenerate.dailyPOSM.isPOSMRepair =
      dailyCmAct.dailyPOSM.isPOSMRepair;
    this.dailyTaskGenerate.dailyPOSM.isPOSMRemoval =
      dailyCmAct.dailyPOSM.isPOSMRemoval;

    this.posmInstallationProducts.forEach((pp) => {
      pp.isSelected = dailyCmAct.dailyPOSM.posmInstallationProducts.find(
        (dp) => dp.productId == pp.id
      )
        ? true
        : false;
    });
    this.posmRepairProducts.forEach((pp) => {
      pp.isSelected = dailyCmAct.dailyPOSM.posmRepairProducts.find(
        (dp) => dp.productId == pp.id
      )
        ? true
        : false;
    });
    this.posmRemovalProducts.forEach((pp) => {
      pp.isSelected = dailyCmAct.dailyPOSM.posmRemovalProducts.find(
        (dp) => dp.productId == pp.id
      )
        ? true
        : false;
    });
    //#endregion

    //#region Audit
    this.dailyTaskGenerate.dailyAudit.isDistributionCheck =
      dailyCmAct.dailyAudit.isDistributionCheck;
    this.dailyTaskGenerate.dailyAudit.isFacingCount =
      dailyCmAct.dailyAudit.isFacingCount;
    this.dailyTaskGenerate.dailyAudit.isPlanogramCheck =
      dailyCmAct.dailyAudit.isPlanogramCheck;
    this.dailyTaskGenerate.dailyAudit.isPriceAudit =
      dailyCmAct.dailyAudit.isPriceAudit;

    this.distributionCheckProducts.forEach((pp) => {
      pp.isSelected = dailyCmAct.dailyAudit.distributionCheckProducts.find(
        (dp) => dp.productId == pp.id
      )
        ? true
        : false;
    });
    this.facingCountProducts.forEach((pp) => {
      pp.isSelected = dailyCmAct.dailyAudit.facingCountProducts.find(
        (dp) => dp.productId == pp.id
      )
        ? true
        : false;
    });
    this.planogramCheckProducts.forEach((pp) => {
      pp.isSelected = dailyCmAct.dailyAudit.planogramCheckProducts.find(
        (dp) => dp.pOSMProductId == pp.id
      )
        ? true
        : false;
    });
    this.priceAuditProducts.forEach((pp) => {
      pp.isSelected = dailyCmAct.dailyAudit.priceAuditProducts.find(
        (dp) => dp.productId == pp.id
      )
        ? true
        : false;
    });
    //#endregion

    //#region Survey
    this.surveys.forEach((pp) => {
      pp.isSelected = dailyCmAct.surveys.find((dp) => dp.id == pp.id)
        ? true
        : false;
    });

    this.consumerSurveys.forEach((pp) => {
      pp.isSelected = dailyCmAct.consumerSurveys.find((dp) => dp.id == pp.id)
        ? true
        : false;
    });
    //#endregion
  }

  //#endregion

  //#region helper method
  validateDate(date: NgbDateStruct | null): NgbDate | null {
    return date && this.calendar.isValid(NgbDate.from(date))
      ? NgbDate.from(date)
      : null;
  }

  dateToString(date: NgbDateStruct): string {
    if (this.validateDate(date)) {
      return `${date.year}-${date.month}-${date.day}`;
    }
    return "";
  }

  getWithoutRef(value) {
    return JSON.parse(JSON.stringify(value));
  }
  //#endregion

  public submitDailyCmTaskForm() {
    if (this.dailyTaskGenerates.length > 0) {
      console.log("submit daily cm task: ", this.dailyTaskGenerates);
      this.alertService.fnLoading(true);
      this.cmTaskGenerationService
        .saveCMTask(this.dailyTaskGenerates)
        .pipe(
          finalize(() => {
            this.alertService.fnLoading(false);
          })
        )
        .subscribe((res) => {
          console.log(res);
          this.alertService.tosterSuccess(`Cm Task has been saved successfully,<br/> ${res.msg}`);
          this.router.navigate(['/task/cm-task-list']);
        });
    }
  }

  openCmTaskDetailsModal(dailyCmAct: DailyCMActivity) {
    let ngbModalOptions: NgbModalOptions = {
      backdrop: "static",
      keyboard: false,
      size: "lg",
    };
    const modalRef = this.modalService.open(
      ModalCmTaskDetailsComponent,
      ngbModalOptions
    );
    modalRef.componentInstance.dailyCmActivity = dailyCmAct;
    modalRef.componentInstance.posmInstallationProducts = this.posmInstallationProducts.filter(
      (pp) => {
        return (
          dailyCmAct.dailyPOSM.posmInstallationProducts.find(
            (dp) => dp.productId == pp.id
          ) || false
        );
      }
    );
    modalRef.componentInstance.posmRepairProducts = this.posmRepairProducts.filter(
      (pp) => {
        return (
          dailyCmAct.dailyPOSM.posmRepairProducts.find(
            (dp) => dp.productId == pp.id
          ) || false
        );
      }
    );
    modalRef.componentInstance.posmRemovalProducts = this.posmRemovalProducts.filter(
      (pp) => {
        return (
          dailyCmAct.dailyPOSM.posmRemovalProducts.find(
            (dp) => dp.productId == pp.id
          ) || false
        );
      }
    );
    modalRef.componentInstance.distributionCheckProducts = this.distributionCheckProducts.filter(
      (pp) => {
        return (
          dailyCmAct.dailyAudit.distributionCheckProducts.find(
            (dp) => dp.productId == pp.id
          ) || false
        );
      }
    );
    modalRef.componentInstance.facingCountProducts = this.facingCountProducts.filter(
      (pp) => {
        return (
          dailyCmAct.dailyAudit.facingCountProducts.find(
            (dp) => dp.productId == pp.id
          ) || false
        );
      }
    );
    modalRef.componentInstance.planogramCheckProducts = this.planogramCheckProducts.filter(
      (pp) => {
        return (
          dailyCmAct.dailyAudit.planogramCheckProducts.find(
            (dp) => dp.pOSMProductId == pp.id
          ) || false
        );
      }
    );
    modalRef.componentInstance.priceAuditProducts = this.priceAuditProducts.filter(
      (pp) => {
        return (
          dailyCmAct.dailyAudit.priceAuditProducts.find(
            (dp) => dp.productId == pp.id
          ) || false
        );
      }
    );
    modalRef.componentInstance.surveys = this.surveys.filter((pp) => {
      return dailyCmAct.surveys.find((dp) => dp.id == pp.id) || false;
    });
    modalRef.componentInstance.consumerSurveys = this.consumerSurveys.filter(
      (pp) => {
        return dailyCmAct.consumerSurveys.find((dp) => dp.id == pp.id) || false;
      }
    );

    modalRef.result.then(
      (result) => {
        console.log(result);
      },
      (reason) => {
        console.log(reason);
      }
    );
  }

  //#region task mapping table

  //#region posm installation table
  public posmInstallPtableSettings: IPTableSetting = {
    tableID: "posm-install-table",
    tableClass: "table table-border ",
    tableName: "POSM Products",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "10%", internalName: "isSelected", sort: false, type: "checkbox", onClick:'true' },
      { headerName: "Product Code", width: "35%", internalName: "code", sort: true, type: "" },
      { headerName: "Product Name", width: "35%", internalName: "name", sort: true, type: "" },
      { headerName: "Signature Required", width: "20%", internalName: "isDigitalSignatureEnableLabel", sort: true, type: "" },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 5,
    enabledPagination: true,
    enabledCellClick: true
  };

  public fnPosmInstallPtableCellClick(event) {
    if (event.cellName == "isSelected") {
      let obj = this.posmInstallationProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForPOSM();
    }
  }
  //#endregion

  //#region posm repair table
  public posmRepairPtableSettings: IPTableSetting = {
    tableID: "posm-repair-table",
    tableClass: "table table-border ",
    tableName: "POSM Products",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "10%", internalName: "isSelected", sort: false, type: "checkbox", onClick:'true' },
      { headerName: "Product Code", width: "35%", internalName: "code", sort: true, type: "" },
      { headerName: "Product Name", width: "35%", internalName: "name", sort: true, type: "" },
      { headerName: "Signature Required", width: "20%", internalName: "isDigitalSignatureEnableLabel", sort: true, type: "" },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 5,
    enabledPagination: true,
    enabledCellClick: true
  };

  public fnPosmRepairPtableCellClick(event) {
    if (event.cellName == "isSelected") {
      let obj = this.posmRepairProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForPOSM();
    }
  }
  //#endregion

  //#region posm removal table
  public posmRemovalPtableSettings: IPTableSetting = {
    tableID: "posm-removal-table",
    tableClass: "table table-border ",
    tableName: "POSM Products",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "10%", internalName: "isSelected", sort: false, type: "checkbox", onClick:'true' },
      { headerName: "Product Code", width: "35%", internalName: "code", sort: true, type: "" },
      { headerName: "Product Name", width: "35%", internalName: "name", sort: true, type: "" },
      { headerName: "Signature Required", width: "20%", internalName: "isDigitalSignatureEnableLabel", sort: true, type: "" },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 5,
    enabledPagination: true,
    enabledCellClick: true
  };

  public fnPosmRemovalPtableCellClick(event) {
    if (event.cellName == "isSelected") {
      let obj = this.posmRemovalProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForPOSM();
    }
  }
  //#endregion

  //#region dist check table
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

  public fnDistCheckPtableCellClick(event) {
    if (event.cellName == "isSelected") {
      let obj = this.distributionCheckProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForAudit();
    }
  }
  //#endregion

  //#region facing check table
  public facingCountPtableSettings: IPTableSetting = {
    tableID: "facing-count-table",
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

  public fnFacingCountPtableCellClick(event) {
    if (event.cellName == "isSelected") {
      let obj = this.facingCountProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForAudit();
    }
  }
  //#endregion

  //#region plan check table
  public planCheckPtableSettings: IPTableSetting = {
    tableID: "plan-check-table",
    tableClass: "table table-border ",
    tableName: "POSM Products",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "10%", internalName: "isSelected", sort: false, type: "checkbox", onClick:'true' },
      { headerName: "Product Code", width: "35%", internalName: "code", sort: true, type: "" },
      { headerName: "Product Name", width: "35%", internalName: "name", sort: true, type: "" },
      { headerName: "Signature Required", width: "20%", internalName: "isDigitalSignatureEnableLabel", sort: true, type: "" },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 5,
    enabledPagination: true,
    enabledCellClick: true
  };

  public fnPlanCheckPtableCellClick(event) {
    if (event.cellName == "isSelected") {
      let obj = this.planogramCheckProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForAudit();
    }
  }
  //#endregion

  //#region price audit table
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

  public fnPriceAuditPtableCellClick(event) {
    if (event.cellName == "isSelected") {
      let obj = this.priceAuditProducts.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForAudit();
    }
  }
  //#endregion

  //#region survey check table
  public surveyPtableSettings: IPTableSetting = {
    tableID: "survey-table",
    tableClass: "table table-border ",
    tableName: "Survey",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "20%", internalName: "isSelected", sort: false, type: "checkbox", onClick:'true' },
      { headerName: "Survey Name", width: "80%", internalName: "surveyName", sort: true, type: "" },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 5,
    enabledPagination: true,
    enabledCellClick: true
  };

  public fnSurveyPtableCellClick(event) {
    if (event.cellName == "isSelected") {
      let obj = this.surveys.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForSurvey();
    }
  }
  //#endregion

  //#region cons survey check table
  public consSurveyPtableSettings: IPTableSetting = {
    tableID: "consumer-survey-table",
    tableClass: "table table-border ",
    tableName: "Consumer Survey",
    tableRowIDInternalName: "id",
    tableColDef: [
      { headerName: "Select", width: "20%", internalName: "isSelected", sort: false, type: "checkbox", onClick:'true' },
      { headerName: "Survey Name", width: "80%", internalName: "surveyName", sort: true, type: "" },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 5,
    enabledPagination: true,
    enabledCellClick: true
  };

  public fnConsSurveyPtableCellClick(event) {
    if (event.cellName == "isSelected") {
      let obj = this.consumerSurveys.find(x => x.id == event.record.id);
      obj.isSelected = !obj.isSelected;
      this.updateParentCheckboxForConsumerSurvey();
    }
  }
  //#endregion

  //#endregion

  updateParentCheckboxForPOSM() {
    let posmInstallationCount = 0;
    let posmRepairCount = 0;
    let posmRemovalCount = 0;

    this.posmInstallationProducts.forEach((element) => {
      console.log(element.name, element.isSelected);
      if (element.isSelected == true) {
        posmInstallationCount++;
      }
    });

    if (posmInstallationCount > 0) {
      this.dailyTaskGenerate.dailyPOSM.isPOSMInstallation = true;
    } else {
      this.dailyTaskGenerate.dailyPOSM.isPOSMInstallation = false;
    }

    this.posmRepairProducts.forEach((element) => {
      if (element.isSelected == true) {
        posmRepairCount++;
      }
    });

    if (posmRepairCount > 0) {
      this.dailyTaskGenerate.dailyPOSM.isPOSMRepair = true;
    } else {
      this.dailyTaskGenerate.dailyPOSM.isPOSMRepair = false;
    }

    this.posmRemovalProducts.forEach((element) => {
      if (element.isSelected == true) {
        posmRemovalCount++;
      }
    });

    if (posmRemovalCount > 0) {
      this.dailyTaskGenerate.dailyPOSM.isPOSMRemoval = true;
    } else {
      this.dailyTaskGenerate.dailyPOSM.isPOSMRemoval = false;
    }

    if(posmInstallationCount > 0 || posmRepairCount > 0 || posmRemovalCount> 0){
      this.dailyTaskGenerate.isPOSM = true;
    }
    else{
      this.dailyTaskGenerate.isPOSM = false;
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


  updateParentCheckboxForSurvey() {

    let surveyCount = 0;
    

    this.surveys.forEach((element) => {

      if (element.isSelected == true) {
        surveyCount++;
      }
    });

    if (surveyCount > 0) {
      this.dailyTaskGenerate.isSurvey = true;
    } else {
      this.dailyTaskGenerate.isSurvey = false;
    }

  }


  updateParentCheckboxForConsumerSurvey() {

    let consumerSurveyCount = 0;
    

    this.consumerSurveys.forEach((element) => {

      if (element.isSelected == true) {
        consumerSurveyCount++;
      }
    });

    if (consumerSurveyCount > 0) {
      this.dailyTaskGenerate.isConsumerSurveyActive = true;
    } else {
      this.dailyTaskGenerate.isConsumerSurveyActive = false;
    }

  }

  //#region comment

  //   isHovered(date: NgbDate) {
  //     return this.fromDate && !this.toDate && this.hoveredDate && date.after(this.fromDate) && date.before(this.hoveredDate);
  //   }

  //   isInside(date: NgbDate) {
  //     return this.toDate && date.after(this.fromDate) && date.before(this.toDate);
  //   }

  //   isRange(date: NgbDate) {
  //     return date.equals(this.fromDate) || (this.toDate && date.equals(this.toDate)) || this.isInside(date) || this.isHovered(date);
  //   }

  //   validateInput(currentValue: NgbDate | null, input: string): NgbDate | null {
  //     const parsed = this.formatter.parse(input);
  //     return parsed && this.calendar.isValid(NgbDate.from(parsed)) ? NgbDate.from(parsed) : currentValue;
  //   }

  //   public selectedFMId:number;
  //   selectedCars = [3];
  //   cars = [
  //     { id: 1, name: 'Volvo' },
  //     { id: 2, name: 'Saab', disabled: true },
  //     { id: 3, name: 'Opel' },
  //     { id: 4, name: 'Audi' },
  //   ];
  //   taggedCmId = [3];
  //   cmids = [
  //     { id: 1, name: 'Volvo' },
  //     { id: 2, name: 'Saab', disabled: true },
  //     { id: 3, name: 'Opel' },
  //     { id: 4, name: 'Audi' },
  //   ];

  //   toggleDisabled() {
  //     const car: any = this.cars[1];
  //     car.disabled = !car.disabled;
  //   }

  //   public fnPtableCellClick(event){
  //     console.log("cell click: ",event );
  //   }

  //   public fnCustomrTrigger(event){
  //     console.log("custom  click: ",event );
  //   }

  //   public ptableSettings = {
  //     tableID: "Employee-table",
  //     tableClass: "table table-border ",
  //     tableName: 'Generated Task For CM',
  //     tableRowIDInternalName: "CarId",
  //     tableColDef: [
  //       { headerName: 'Task Id', width: '10%', internalName: 'employeeId', sort: true, type: "" },
  //       { headerName: 'Date ', width: '15%', internalName: 'date', sort: true, type: "" },
  //       { headerName: 'CM Name ', width: '10%', internalName: 'employeeName', sort: true, type: "" },
  //       { headerName: 'Employee Type', width: '15%', internalName: 'employeeType', sort: true, type: "" },
  //       { headerName: 'Working Project', width: '10%', internalName: 'workingProject', sort: false, type: "" },
  //       { headerName: 'Designation ', width: '10%', internalName: 'designation', sort: true, type: "" },
  //       { headerName: 'Team Name', width: '20%', internalName: 'teamName', sort: true, type: "" },
  //       { headerName: 'Manager Name', width: '10%', internalName: 'managerName', sort: true, type: "" },
  //       { headerName: 'Details', width: '15%', internalName: 'details', sort: true, type: "button", onClick: 'true',innerBtnIcon:"fa fa-copy" },

  //     ],
  //     enabledSearch: false,
  //     enabledSerialNo: true,
  //     pageSize: 25,
  //     enabledPagination: true,
  //     //enabledAutoScrolled:true,
  //     enabledEditDeleteBtn: true,
  //     enabledCellClick: true,
  //     enabledColumnFilter: true,
  //   };

  // public employeeList=[
  //   {employeeId:"BS-120",employeeName:"Palash Kanti Bachar", date:"04/27/2020",employeeType:"Permanent", workingProject:"FM Application", designation:"Team Lead",teamName:"ASP.NET", managerName:"Rakibul Tanvi", details:"More.."   },
  //   {employeeId:"BS-121",employeeName:"Md. Rabby Hasan", date:"04/27/2020",employeeType:"Permanent", workingProject:"FM Application", designation:"Team Lead",teamName:"ASP.NET", managerName:"Rakibul Tanvi", details:"More.."   },
  //   {employeeId:"BS-122",employeeName:"Md. Ashiquzzaman", date:"04/27/2020",employeeType:"Permanent", workingProject:"FM Application", designation:"Team Lead",teamName:"ASP.NET", managerName:"Rakibul Tanvi", details:"More.."   },
  //   {employeeId:"BS-123",employeeName:"Md. Nizam", date:"04/27/2020",employeeType:"Permanent", workingProject:"FM Application", designation:"Team Lead",teamName:"ASP.NET", managerName:"Rakibul Tanvi", details:"More.."   },
  //   {employeeId:"BS-128",employeeName:"Mr Shohel", date:"04/27/2020",employeeType:"Permanent", workingProject:"FM Application", designation:"Team Lead",teamName:"ASP.NET", managerName:"Rakibul Tanvi", details:"More.."   },
  //   {employeeId:"BS-135",employeeName:"Mr Ali 7", date:"04/27/2020",employeeType:"Permanent", workingProject:"FM Application", designation:"Team Lead",teamName:"ASP.NET", managerName:"Rakibul Tanvi", details:"More.."   },
  //   {employeeId:"BS-135",employeeName:"Mr Ali 7", date:"04/27/2020",employeeType:"Permanent", workingProject:"FM Application", designation:"Team Lead",teamName:"ASP.NET", managerName:"Rakibul Tanvi", details:"More.."   },
  // ];

  //#endregion
}
