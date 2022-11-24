import { Component, OnInit, ViewChild } from '@angular/core';
import { Paginator } from 'primeng/paginator';
import { DailyTask } from 'src/app/Shared/Entity/DailyTasks';
import { CommunicationReportTableData } from 'src/app/Shared/Entity/Reports/communicationReportTableData';
import { ReportConst } from 'src/app/Shared/Entity/Reports/ReportConst';
import { IDateRange } from 'src/app/Shared/interfaces';
import { IPTableSetting, colDef } from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ReportService } from 'src/app/Shared/Services/Reports/report-service';
import { Utility } from 'src/app/Shared/utility';
import { ReportUtility } from 'src/app/Shared/Utility/reportUtility';

@Component({
  selector: 'app-communication-report',
  templateUrl: './communication-report.component.html',
  styleUrls: ['./communication-report.component.css']
})
export class CommunicationReportComponent implements OnInit {

  reports:DailyTask[]=[];
  rows:CommunicationReportTableData[]=[];

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
    this.reportService.getCommunicationReport({
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
    const rows:CommunicationReportTableData[]=[];
    for(let task of this.reports){
        for(let comTask of task.dailyCommunicationTasks){          
              const row = new CommunicationReportTableData();
              if(task.cmUser) row.cmUserName = task.cmUser.name
              row.displayDate = Utility.getDateToStringFormat(task.dateTimeStr);
              row.displayStatus = comTask.isCompleted?"Complete":"InComplete";
              if(task.salesPoint) row.salesPointName = task.salesPoint.name;
              if(comTask.outlet) {
                row.outletName = comTask.outlet.name;
                row.outletCode = comTask.outlet.code;
                if(comTask.outlet.route){
                  row.route = comTask.outlet.route.routeName;
                }
              }
              if(!comTask.isOutletOpen){                
                ReportUtility.SetOutletClosedStatus(row);
                rows.push(row);
                continue;
              }
              else if(!comTask.isCompleted){                
                ReportUtility.SetIncompleteStatus(row,comTask);                
                rows.push(row);
                continue;
              }
              if(comTask.communicationSetup){
                if(comTask.communicationSetup.avCommunication) row.communicationName = comTask.communicationSetup.avCommunication.campaignName;
              }
              row.displayStatus = ReportConst.Comlete;
              rows.push(row);          
        }
    }
    this.rows = rows;
}

public ptableSettings:IPTableSetting<colDef<keyof CommunicationReportTableData>> = {
  tableID: "complete-report-table",
  tableClass: "table-responsive",
  tableName: 'Communication Report List',
  tableRowIDInternalName: "Id",
  tableColDef: [
      { headerName: 'Date', width: '10%', internalName: 'displayDate', sort: true, type: "" },
      { headerName: 'CM User', width: '10%', internalName: 'cmUserName', sort: true, type: "" },
      { headerName: 'Sales Point', width: '10%', internalName: 'salesPointName', sort: true, type: "" },
      { headerName: 'Route', width: '10%', internalName: 'route', sort: true, type: "" },
      { headerName: 'Outlet', width: '15%', internalName: 'outletName', sort: true, type: "" },
      { headerName: 'Outlet Code', width: '10%', internalName: 'outletCode', sort: true, type: "" },
      { headerName: 'Communication name', width: '15%', internalName: 'communicationName', sort: true, type: "" },
      { headerName: 'Status', width: '10%', internalName: 'displayStatus', sort: true, type: "" },
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
    to:this.toDate
  }
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
handleDateRange(dateRange:IDateRange){      
  Utility.adjustDateRange(dateRange);        
  this.fromDate = dateRange.from;
  this.toDate = dateRange.to;
  this.paging.changePage(0);              
}
fnCustomTrigger(event) {

}
handleExport(){
  let fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
  this.reportService.exportCommunicationReportToExcel({
    fromDateTime:this.fromDate,
    toDateTime:this.toDate,
    pageIndex:1,
    pageSize:Utility.MaximumWorkableInteger,
    search:""
  }).subscribe(data => {
      const fileName = `Communication_Report_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
      this.commonService.DownloadFile(data, fileName, fileType);
  });

}
handlePazeSizeChange(pageSize:number){
  this.pageSize = pageSize;
  this.paging.changePage(0);
}
}
