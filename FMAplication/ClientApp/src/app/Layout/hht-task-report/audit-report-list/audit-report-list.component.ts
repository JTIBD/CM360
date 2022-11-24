import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Paginator } from 'primeng/paginator';
import { DailyTask } from 'src/app/Shared/Entity/DailyTasks';
import { AuditReportTableData } from 'src/app/Shared/Entity/Reports';
import { ReportConst } from 'src/app/Shared/Entity/Reports/ReportConst';
import { ActionTypeLabel, ActionType } from 'src/app/Shared/Enums/actionType';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { IDateRange } from 'src/app/Shared/interfaces';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { IPTableSetting, colDef } from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { CmTaskGenerationService } from 'src/app/Shared/Services/DailyActivity/cm-task-generation.service';
import { ReportService } from 'src/app/Shared/Services/Reports/report-service';
import { Utility } from 'src/app/Shared/utility';
import { ReportUtility } from 'src/app/Shared/Utility/reportUtility';

@Component({
  selector: 'app-audit-report-list',
  templateUrl: './audit-report-list.component.html',
  styleUrls: ['./audit-report-list.component.css']
})
export class AuditReportListComponent implements OnInit {

  tosterMsgError: string = "Something went wrong!";
  // public auditReportList:AuditReport[]=[];
  public auditReportList:any[]=[];
  tableRows:AuditReportTableData[]=[];
  reports:DailyTask[]=[];
  mapObject : MapObject;
  enumStatusTypes : MapObject[] = StatusTypes.statusType;
    enumActionTypes: MapObject[] = ActionTypeLabel.ActionType;

    search: string = "";
    pageIndex = 1;
    pageSize = 10;
    total: number;
    fromDate = Utility.getFirstDateOfCurrentMonth();
    toDate=  new Date().toISOString();
    @ViewChild("paging", { static: false }) paging: Paginator;

  constructor(private cmTaskGenerationService: CmTaskGenerationService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private alertService: AlertService,
    private reportService: ReportService,
    private commonService:CommonService,
    ) { }

  ngOnInit() {
      this.getReports();
  }

  ngAfterViewInit() {
    this.enableCurrentPageReport();
  }

  enableCurrentPageReport() {
    let timer = setInterval(() => {
      if (this.paging) {
        this.paging.showCurrentPageReport = true;
        clearInterval(timer);
      }
    }, 1);
  }

  getPaginationStatus() {
    return Utility.getPaginationStatus(this.total, this.pageIndex, this.pageSize,this.tableRows.length);
  }


  getReports(){
    this.reportService.getAuditReports({
      fromDateTime:this.fromDate,
      toDateTime:this.toDate,
      pageIndex:this.pageIndex,
      pageSize:this.pageSize,
      search:this.search
    }).subscribe(res=>{
      this.reports = res.data;
      this.total = res.count;
      this.mapToTable();
      this.paging.updatePaginatorState();
    })
  }

  mapToTable(){
    const rows:AuditReportTableData[]=[];
        for(let task of this.reports){
            for(let posmTask of task.dailyAuditTasks){

              let createRow=()=>{
                const rowTemp = new AuditReportTableData();
                if(task.cmUser) rowTemp.cmUserName = task.cmUser.name;
                rowTemp.displayDate = Utility.getDateToStringFormat(task.dateTimeStr);
                if(posmTask.outlet) {
                  rowTemp.outletName = posmTask.outlet.name;
                  rowTemp.outletCode = posmTask.outlet.code;
                  if(posmTask.outlet.route){
                    rowTemp.route = posmTask.outlet.route.routeName;
                  }
                }                
                if(task.salesPoint) rowTemp.salesPointName = task.salesPoint.name;
                return rowTemp;
              }

              if(!posmTask.isOutletOpen){
                const row = createRow();
                ReportUtility.SetOutletClosedStatus(row);
                rows.push(row);
                continue;
              }     

              let displayStatus = ReportConst.Comlete;
              let reason = "";
              if(!posmTask.isCompleted){
                displayStatus = ReportConst.InCompleteReason;
                if(posmTask.reason) reason = posmTask.reason.reasonInEnglish;
              }          

               let distributionChecked = posmTask.dailyProductsAuditTask.filter(x=>x.actionType === ActionType.DistributionCheckProduct);
               for(let item of distributionChecked){
                let row = createRow();
                row.amount = item.result;
                row.displayActionType = ActionTypeLabel.ActionType.find(x=>x.id === ActionType.DistributionCheckProduct).label;
                if(item.product) row.productName = item.product.name;
                row.displayStatus = displayStatus;
                row.reason = reason;
                rows.push(row);
               }
               

               let priceAudited = posmTask.dailyProductsAuditTask.filter(x=>x.actionType === ActionType.PriceAuditProduct);
               for(let item of priceAudited){
                let row = createRow();
                row.amount = item.result;
                row.displayActionType = ActionTypeLabel.ActionType.find(x=>x.id === ActionType.PriceAuditProduct).label;
                if(item.product) row.productName = item.product.name;
                row.displayStatus = displayStatus;
                row.reason = reason;
                rows.push(row);
               }

               let faceCounted = posmTask.dailyProductsAuditTask.filter(x=>x.actionType === ActionType.FacingCountProduct);
               for(let item of faceCounted){
                let row = createRow();
                row.amount = item.result;
                row.displayActionType = ActionTypeLabel.ActionType.find(x=>x.id === ActionType.FacingCountProduct).label;
                if(item.posmProduct) row.productName = item.posmProduct.name;
                row.displayStatus = displayStatus;
                row.reason = reason;
                rows.push(row);
               }

               let planogramChecked = posmTask.dailyProductsAuditTask.filter(x=>x.actionType === ActionType.PlanogramCheckProduct);
               for(let item of planogramChecked){
                let row = createRow();
                if(item.result == 1) row.amount = "No";
                else if(item.result == 2) row.amount = "Yes";
                row.displayActionType = ActionTypeLabel.ActionType.find(x=>x.id === ActionType.PlanogramCheckProduct).label;
                if(item.posmProduct) row.productName = item.posmProduct.name;
                row.displayStatus = displayStatus;
                row.reason = reason;
                rows.push(row);
               }
                                             
            }
        }
        this.tableRows = rows;
  }


    getAllReportData(pageIndex, pageSize, search) {
    this.alertService.fnLoading(true);
        this.cmTaskGenerationService.getAuditReports(pageIndex, pageSize, search).subscribe(
      (res: any) => {
                this.auditReportList = res.data.item1;
                this.total = res.data.item2;
                this.paging.totalRecords = this.total;
                this.paging.showCurrentPageReport = true;        
      },
      (error) => {
        this.alertService.fnLoading(false);
        this.alertService.tosterDanger(this.tosterMsgError);
      },
      () => this.alertService.fnLoading(false));
  }
    fnSearch($event: any) {
      this.search = $event.searchVal;
      this.paging.changePage(0);
    }
    paginate(event) {
      this.pageIndex = Number(event.page) + 1;    
      this.getReports();
      this.ptableSettings.serverSitePageIndex = this.pageIndex;
    }
    reset() {
        this.pageIndex = 1;
        this.paging.changePage(0);
    }
    handleDateRange(dateRange:IDateRange){      
      Utility.adjustDateRange(dateRange);        
      this.fromDate = dateRange.from;
      this.toDate = dateRange.to;
      this.paging.changePage(0);              
    }

    handleExport(){
      let fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
      this.reportService.exportAuditReportToExcel({
        fromDateTime:this.fromDate,
        toDateTime:this.toDate,
        pageIndex:1,
        pageSize:Utility.MaximumWorkableInteger,
        search:""
      }).subscribe(data => {
          const fileName = `AuditReport_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
          this.commonService.DownloadFile(data, fileName, fileType);
      });

    }

  public ptableSettings:IPTableSetting<colDef<keyof AuditReportTableData>> = {
    tableID: "complete-report-table",
    tableClass: "table-responsive",
    tableName: 'Audit Report List',
    tableRowIDInternalName: "Id",
    tableColDef: [
          { headerName: 'Date', width: '10%', internalName: 'displayDate', sort: true, type: "" },
          { headerName: 'CM User', width: '10%', internalName: 'cmUserName', sort: true, type: "" },
          { headerName: 'Sales Point', width: '10%', internalName: 'salesPointName', sort: true, type: "" },
          { headerName: 'Route', width: '10%', internalName: 'route', sort: true, type: "" },
          { headerName: 'Outlet', width: '10%', internalName: 'outletName', sort: true, type: "" },
          { headerName: 'Outlet Code', width: '9%', internalName: 'outletCode', sort: true, type: "" },
          { headerName: 'Product Name', width: '10%', internalName: 'productName', sort: true, type: "" },
          { headerName: 'Amount', width: '7%', internalName: 'amount', sort: true, type: "" },
          { headerName: 'Action Type', width: '7%', internalName: 'displayActionType', sort: true, type: "" },
          { headerName: 'Status', width: '7%', internalName: 'displayStatus', sort: true, type: "" },
          { headerName: 'Reason', width: '10%', internalName: 'reason', sort: true, type: "" },
    
        ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 10,
    enabledPagination: true,
    enabledColumnFilter: true, 
    enabledRecordCreateBtn: false,
    enabledServerSitePaggination: true,
    tableFooterVisibility: false,
    enableDateRangeFilter:true,
    enabledDataLength:true,
    intialDateRange:{
      from:this.fromDate,
      to: this.toDate
    }
  };

  fnCustomTrigger(event){

  }

  handlePazeSizeChange(pageSize:number){
    this.pageSize = pageSize;
    this.paging.changePage(0);
  }
}
