import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Paginator } from 'primeng/paginator';
import { DailyTask } from 'src/app/Shared/Entity/DailyTasks';
import { InformationReportTableData } from 'src/app/Shared/Entity/Reports/informationReportTableData';
import { ReportConst } from 'src/app/Shared/Entity/Reports/ReportConst';
import { IDateRange } from 'src/app/Shared/interfaces';
import { ImageViewModalComponent } from 'src/app/Shared/Modules/image-view-modal/image-view-modal.component';
import { IPTableSetting, colDef } from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ReportService } from 'src/app/Shared/Services/Reports/report-service';
import { Utility } from 'src/app/Shared/utility';
import { ReportUtility } from 'src/app/Shared/Utility/reportUtility';

@Component({
  selector: 'app-information-report',
  templateUrl: './information-report.component.html',
  styleUrls: ['./information-report.component.css']
})
export class InformationReportComponent implements OnInit {

  

  reports:DailyTask[]=[];
  rows:InformationReportTableData[]=[];

  pageIndex=1;
  pageSize=10;
  search="";
  total=Number.MAX_SAFE_INTEGER;
  fromDate = Utility.getFirstDateOfCurrentMonth();
  toDate=  new Date().toISOString();
  @ViewChild("paging", { static: false }) paging: Paginator;
  constructor(
    private reportService:ReportService,
    private commonService:CommonService,
    private modalService: NgbModal
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
    return Utility.getPaginationStatus(this.total, this.pageIndex, this.pageSize,this.rows.length);
  }


  getReports(){
    this.reportService.getInformationReport({
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
    const rows:InformationReportTableData[]=[];
    for(let task of this.reports){
        for(let infoTask of task.dailyInformationTasks){          


              const row = new InformationReportTableData();
              if(task.cmUser) row.cmUserName = task.cmUser.name
              row.displayDate = Utility.getDateToStringFormat(task.dateTimeStr);
              if(task.salesPoint) row.salesPointName = task.salesPoint.name;
              if(infoTask.outlet) {
                row.outletName = infoTask.outlet.name;
                row.outletCode = infoTask.outlet.code;
                if(infoTask.outlet.route){
                  row.route = infoTask.outlet.route.routeName;
                }
              }
              
              if(!infoTask.isOutletOpen){
                ReportUtility.SetOutletClosedStatus(row);
                rows.push(row);
                continue;
              }
              else if(!infoTask.isCompleted){
                  ReportUtility.SetIncompleteStatus(row,infoTask);
                  rows.push(row);
                  continue;
              }
              row.displayStatus = ReportConst.Comlete;              

              if (infoTask.requestImage != "") {
                row.requestImage = infoTask.requestImage;
                row.viewRequestImageBtnText = "view";
              };

              if (infoTask.insightImage != "") {
                row.insightImage = infoTask.insightImage;
                row.viewInsightImageBtnText = "view";
              }

              row.insight = infoTask.insightDescription; 
              row.request = infoTask.requestDescription; 

              // if(avTask.communicationSetup){
              //   if(avTask.communicationSetup.avCommunication) row.communicationName = avTask.communicationSetup.avCommunication.campaignName;
              // }
              rows.push(row);          
        }
    }
    this.rows = rows;
}

public ptableSettings:IPTableSetting<colDef<keyof InformationReportTableData>> = {
  tableID: "complete-report-table",
  tableClass: "table-responsive",
  tableName: 'Information Report List',
  tableRowIDInternalName: "Id",
  tableColDef: [
      { headerName: 'Date', width: '8%', internalName: 'displayDate', sort: true, type: "" },
      { headerName: 'CM User', width: '8%', internalName: 'cmUserName', sort: true, type: "" },
      { headerName: 'Sales Point', width: '8%', internalName: 'salesPointName', sort: true, type: "" },      
      { headerName: 'Route', width: '8%', internalName: 'route', sort: true, type: "" },
      { headerName: 'Outlet', width: '8%', internalName: 'outletName', sort: true, type: "" },      
      { headerName: 'Outlet Code', width: '8%', internalName: 'outletCode', sort: true, type: "" },      
      { headerName: 'Insight', width: '10%', internalName: 'insight', sort: true, type: "" },      
      { headerName: 'Insight Image', width: '8%', internalName: 'viewInsightImageBtnText', sort: false, type: "button", onClick: 'true', innerBtnIcon:"fa fa-info" },      
      { headerName: 'Request', width: '10%', internalName: 'request', sort: true, type: "" },      
      { headerName: 'Request Image', width: '8%', internalName: 'viewRequestImageBtnText', sort: false, type: "button", onClick: 'true', innerBtnIcon:"fa fa-info" },      
      { headerName: 'Status', width: '8%', internalName: 'displayStatus', sort: true, type: "" },
      { headerName: 'Reason', width: '8%', internalName: 'reason', sort: true, type: "" },

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
  enabledCellClick:true,
  intialDateRange:{
    from:this.fromDate,
    to:this.toDate,
  },
  enabledDataLength:true,
};

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
  this.reportService.exportInformationReportToExcel({
    fromDateTime:this.fromDate,
    toDateTime:this.toDate,
    pageIndex:1,
    pageSize:Utility.MaximumWorkableInteger,
    search:""
  }).subscribe(data => {
      const fileName = `Information_Report_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
      this.commonService.DownloadFile(data, fileName, fileType);
  });

}

  fnPtableCellClick(event: any) {
    console.log("cell click: ", event);
    let imageSrc = ""; 
    let imageTitle = "";
    
    if (event.cellName === "viewInsightImageBtnText" || 
    event.cellName === "viewRequestImageBtnText" ) {

    
    if (event.cellName === 'viewInsightImageBtnText' && event.record.insightImage != ""){
        imageSrc = event.record.insightImage;
        imageTitle = 'Insight Image';
    }
    else if (event.cellName === 'viewRequestImageBtnText' && event.record.requestImage != ""){
        imageSrc = event.record.requestImage;
        imageTitle = 'Request Image';
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
