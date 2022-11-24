import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Paginator } from 'primeng/paginator';
import { DailyPOSMReportTableData } from 'src/app/Shared/Entity';
import { DailyTask } from 'src/app/Shared/Entity/DailyTasks';
import { ReportConst } from 'src/app/Shared/Entity/Reports/ReportConst';
import { PosmWorkTypeLabels } from 'src/app/Shared/Enums';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { POSMActionTypeLabel } from 'src/app/Shared/Enums/posmActionType';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { IDateRange } from 'src/app/Shared/interfaces';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { ImageViewModalComponent } from 'src/app/Shared/Modules/image-view-modal/image-view-modal.component';
import { IPTableSetting, colDef } from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ReportService } from 'src/app/Shared/Services/Reports/report-service';
import { Utility } from 'src/app/Shared/utility';
import { ReportUtility } from 'src/app/Shared/Utility/reportUtility';

@Component({
  selector: 'app-posm-report',
  templateUrl: './posm-report.component.html',
  styleUrls: ['./posm-report.component.css']
})
export class PosmReportComponent implements OnInit {



  tosterMsgError: string = "Something went wrong!";
  // public posmReportList:PosmReport[]=[];
  public posmReportList: any[] = [];
  public submitedPosmTasks: DailyTask[] = [];
  public tableRows:DailyPOSMReportTableData[]=[];
  mapObject: MapObject;
  enumStatusTypes: MapObject[] = StatusTypes.statusType;
  enumActionTypes: MapObject[] = POSMActionTypeLabel.POSMActionType;

  search: string = "";
  pageIndex = 1;
  pageSize = 10;
  total: number;
  fromDate = Utility.getFirstDateOfCurrentMonth();
  toDate=  new Date().toISOString();
  @ViewChild("paging", { static: false }) paging: Paginator;

  constructor(
      private router: Router,
      private activatedRoute: ActivatedRoute,
      private alertService: AlertService,
      private reportService: ReportService,
      private commonService:CommonService, 
      private modalService: NgbModal,
      ) { }

  ngOnInit() {
      this.getSubmittedPosmTasks();
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


  getSubmittedPosmTasks(){
      this.reportService.getPOSMReport({
          fromDateTime:this.fromDate,
          toDateTime:this.toDate,
          pageIndex:this.pageIndex,
          pageSize:this.pageSize,
          search:this.search
      }).subscribe(res=>{
          this.submitedPosmTasks = res.data;
          this.total = res.count;
          this.mapToTableData();
          this.paging.updatePaginatorState();
      })
  }

  mapToTableData(){
      const rows:DailyPOSMReportTableData[]=[];
      for(let task of this.submitedPosmTasks){
          for(let posmTask of task.dailyPosmTasks){
              
            const base = new DailyPOSMReportTableData();
            if(task.cmUser) base.cmUserName = task.cmUser.name
            base.displayDate = Utility.getDateToStringFormat(task.dateTimeStr);
            if(posmTask.outlet) {
              base.outletName = posmTask.outlet.name;
              base.outletCode = posmTask.outlet.code;
              if(posmTask.outlet.route){
                base.route = posmTask.outlet.route.routeName;
              }
            }
            if(task.salesPoint) base.salesPointName = task.salesPoint.name;
           
            

            if(!posmTask.isOutletOpen){
                const row = {...base};
                ReportUtility.SetOutletClosedStatus(base);                
                rows.push(base);
                continue;
            }
            else if(!posmTask.isCompleted){
                ReportUtility.SetIncompleteStatus(base,posmTask);    

                rows.push(base);
                continue;
            }

            for(let item of posmTask.dailyPosmTaskItems){
                const row = {...base};
                row.displayStatus = ReportConst.Comlete;
                row.amount = item.quantity;
                row.displayActionType = PosmWorkTypeLabels[item.executionType];

                if (item.image && item.image != ""){
                    row.productImage =item.image;
                    row.viewButtonTextForProductImage =  "View"; 
                }

                if (posmTask.newImage && posmTask.newImage != ""){
                    row.newImage =  posmTask.newImage;
                    row.viewButtonTextForNewImage =  "View"  ;
                }

                if (posmTask.existingImage && posmTask.existingImage != ""){
                    row.existingImage =  posmTask.existingImage;
                    row.viewButtonTextForExistingImage =  "View";
                }

                if(item.posmProduct) row.productName = item.posmProduct.name;
                rows.push(row);
            }
          }
      }
      this.tableRows = rows;
  }

  fnSearch($event: any) {
      this.search = $event.searchVal;
      this.paging.changePage(0);         
  }
  paginate(event) {
      this.pageIndex = Number(event.page) + 1;    
      this.getSubmittedPosmTasks();
      this.ptableSettings.serverSitePageIndex = this.pageIndex;
      
  }
  reset() {
      this.pageIndex = 1;
      this.paging.changePage(0);
  }
  public ptableSettings: IPTableSetting<colDef<keyof DailyPOSMReportTableData>> = {
      tableID: "complete-report-table",
      tableClass: "table-responsive",
      tableName: 'POSM Report List',
      tableRowIDInternalName: "Id",
      tableColDef: [
          { headerName: 'Date', width: '8%', internalName: 'displayDate', sort: true, type: "" },
          { headerName: 'CM User', width: '7%', internalName: 'cmUserName', sort: true, type: "" },
          { headerName: 'Sales Point', width: '7%', internalName: 'salesPointName', sort: true, type: "" },
          { headerName: 'Route', width: '7%', internalName: 'route', sort: true, type: "" },
          { headerName: 'Outlet', width: '7%', internalName: 'outletName', sort: true, type: "" },
          { headerName: 'Outlet Code', width: '7%', internalName: 'outletCode', sort: true, type: "" },
          { headerName: 'Exsiting Status', width: '7%', internalName: 'viewButtonTextForExistingImage', sort: false, type: "button", onClick: 'true', innerBtnIcon:"fa fa-info" },
          { headerName: 'Product Name', width: '8%', internalName: 'productName', sort: true, type: "" },
          { headerName: 'Product Image', width: '7%', internalName: 'viewButtonTextForProductImage', sort: false, type: "button", onClick: 'true', innerBtnIcon:"fa fa-info" },
          { headerName: 'Amount', width: '7%', internalName: 'amount', sort: true, type: "" },
          { headerName: 'Action Type', width: '7%', internalName: 'displayActionType', sort: true, type: "" },
          { headerName: 'New Image', width: '7%', internalName: 'viewButtonTextForNewImage', sort: false, type: "button", onClick: 'true', innerBtnIcon:"fa fa-info" },
          { headerName: 'Status', width: '7%', internalName: 'displayStatus', sort: false, type: "" },
          { headerName: 'Reason', width: '8%', internalName: 'reason', sort: true, type: "" },

      ],
      enabledSearch: true,
      enabledSerialNo: true,
      pageSize: 10,
    //  enabledPagination: true,
      enabledColumnFilter: true,
      enabledRecordCreateBtn: false,
      enabledServerSitePaggination: true,
      tableFooterVisibility: false,
      enableDateRangeFilter:true,
      intialDateRange:{
          from:this.fromDate,
          to:this.toDate
      }, 
      enabledCellClick:true,
      enabledDataLength:true,
  };

  fnCustomTrigger(event) {

  }

  
handleDateRange(dateRange:IDateRange){      
    Utility.adjustDateRange(dateRange);        
    this.fromDate = dateRange.from;
    this.toDate = dateRange.to;
    this.paging.changePage(0);              
  }

  handleExport(){
    let fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    this.reportService.exportPOSMTaskReportToExcel({
      fromDateTime:this.fromDate,
      toDateTime:this.toDate,
      pageIndex:1,
      pageSize:Utility.MaximumWorkableInteger,
      search:""
    }).subscribe(data => {
        const fileName = `POSMTask_Report_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
        this.commonService.DownloadFile(data, fileName, fileType);
    });
  
  }
  fnPtableCellClick(event: any) {
    console.log("cell click: ", event);
    let imageSrc = ""; 
    let imageTitle = "";
    if (event.cellName === "viewButtonTextForNewImage" || 
        event.cellName === "viewButtonTextForProductImage" || 
        event.cellName === "viewButtonTextForExistingImage" ) {

        
        if (event.cellName === 'viewButtonTextForNewImage' && event.record.newImage != ""){
            imageSrc = event.record.newImage;
            imageTitle = 'new image';
        }
        else if (event.cellName === 'viewButtonTextForProductImage' && event.record.productImage != ""){
            imageSrc = event.record.productImage;
            imageTitle = 'product image';
        }
        else if (event.cellName === 'viewButtonTextForExistingImage' && event.record.existingImage != ""){
            imageSrc = event.record.existingImage;
            imageTitle = 'product image';
        }


        let ngbModalOptions: NgbModalOptions = {
            backdrop: "static",
            keyboard: false,
            size: "lg",
            };
            const modalRef = this.modalService.open(
            ImageViewModalComponent,
            ngbModalOptions
            );
            
            modalRef.componentInstance.imageSrc = imageSrc;
            modalRef.componentInstance.imageTitle = imageTitle;
            modalRef.result.then(
            (result) => {
                imageSrc = ""; 
                imageTitle = "";
            },
            (reason) => {
                console.log(reason);
            }
            );

    }
  }

  handlePazeSizeChange(pageSize:number){
    this.pageSize = pageSize;
    this.paging.changePage(0);
  }  

}
