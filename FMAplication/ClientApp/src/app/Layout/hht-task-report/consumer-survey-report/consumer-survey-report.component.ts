import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Paginator } from 'primeng/paginator';
import { DailyTask } from 'src/app/Shared/Entity/DailyTasks';
import { ConsumerSurveyReportTableData } from 'src/app/Shared/Entity/Reports/consumerSurveyReportTableData';
import { IDateRange } from 'src/app/Shared/interfaces';
import { ImageViewModalComponent } from 'src/app/Shared/Modules/image-view-modal/image-view-modal.component';
import { IPTableSetting, colDef } from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ReportService } from 'src/app/Shared/Services/Reports/report-service';
import { Utility } from 'src/app/Shared/utility';

@Component({
  selector: 'app-consumer-survey-report',
  templateUrl: './consumer-survey-report.component.html',
  styleUrls: ['./consumer-survey-report.component.css']
})
export class ConsumerSurveyReportComponent implements OnInit {

  reports:DailyTask[]=[];
  rows:ConsumerSurveyReportTableData[]=[];

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
    private modalService: NgbModal,
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
    this.reportService.getConsumerSurveyReports({
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
    const rows:ConsumerSurveyReportTableData[]=[];
    for(let task of this.reports){
        for(let surveyTask of task.dailyConsumerSurveyTasks){
            for(let TaskAnswer of surveyTask.dailyConsumerSurveyTaskAnswers){
                const row = new ConsumerSurveyReportTableData();
                if(task.cmUser) row.cmUserName = task.cmUser.name
                row.answer = TaskAnswer.answer;
                row.displayDate = Utility.getDateToStringFormat(task.dateTimeStr);
                if(TaskAnswer.question) {
                  row.questionName = TaskAnswer.question.questionTitle;
                  if(TaskAnswer.question.questionType === "Signature") row.btnTitle = "View Signature";
                }
                if(task.salesPoint) row.salesPointName = task.salesPoint.name;
                if(surveyTask.surveyQuestionSet) row.surveyName = surveyTask.surveyQuestionSet.name;
                rows.push(row);
            }
        }
    }
    this.rows = rows;
}

public ptableSettings:IPTableSetting<colDef<keyof ConsumerSurveyReportTableData>> = {
  tableID: "complete-report-table",
  tableClass: "table-responsive",
  tableName: 'Consumer Survey Report List',
  tableRowIDInternalName: "Id",
  tableColDef: [
      { headerName: 'Date', width: '15%', internalName: 'displayDate', sort: true, type: "" },
      { headerName: 'CM User', width: '15%', internalName: 'cmUserName', sort: true, type: "" },
      { headerName: 'Sales Point', width: '15%', internalName: 'salesPointName', sort: true, type: "" },
      { headerName: 'Survey Name', width: '15%', internalName: 'surveyName', sort: true, type: "" },
      { headerName: 'Question', width: '20%', internalName: 'questionName', sort: true, type: "" },
      { headerName: 'Answer', width: '20%', internalName: 'answer', sort: true, type: "buttonAndText",onClick: 'true' },

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
  intialDateRange:{
      from:this.fromDate,
      to:this.toDate,
  },
  enabledCellClick:true,
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
  this.reportService.exportConsumerSurveyReportToExcel({
    fromDateTime:this.fromDate,
    toDateTime:this.toDate,
    pageIndex:1,
    pageSize:Utility.MaximumWorkableInteger,
    search:""
  }).subscribe(data => {
      const fileName = `ConsumerSurveyReport_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
      this.commonService.DownloadFile(data, fileName, fileType);
  });

}

fnPtableCellClick(event: any) {
  console.log("cell click: ", event);
  let imageSrc = ""; 
  let imageTitle = "";
  if (event.cellName === "answer" ) {            
      imageSrc = event.record.answer;
      imageTitle = 'Signature';


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
