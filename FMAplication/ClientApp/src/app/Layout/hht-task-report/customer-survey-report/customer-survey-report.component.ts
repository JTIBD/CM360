import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Paginator } from 'primeng/paginator';
import { DailyTask } from 'src/app/Shared/Entity/DailyTasks';
import { SurveyReportTableData } from 'src/app/Shared/Entity/Reports';
import { ReportConst } from 'src/app/Shared/Entity/Reports/ReportConst';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { IDateRange } from 'src/app/Shared/interfaces';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { ImageViewModalComponent } from 'src/app/Shared/Modules/image-view-modal/image-view-modal.component';
import { IPTableSetting, colDef } from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { CmTaskGenerationService } from 'src/app/Shared/Services/DailyActivity/cm-task-generation.service';
import { ReportService } from 'src/app/Shared/Services/Reports/report-service';
import { Utility } from 'src/app/Shared/utility';
import { ReportUtility } from 'src/app/Shared/Utility/reportUtility';

@Component({
  selector: 'app-customer-survey-report',
  templateUrl: './customer-survey-report.component.html',
  styleUrls: ['./customer-survey-report.component.css']
})
export class CustomerSurveyReportComponent implements OnInit {

  tosterMsgError: string = "Something went wrong!";
    // public surveyReportList:SurveyReport[]=[];
    reports:DailyTask[]=[];
    rows : SurveyReportTableData[]=[];
    public surveyReportList: any[] = [];
    mapObject: MapObject;
    enumStatusTypes: MapObject[] = StatusTypes.statusType;

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
        }, 10);
      }
    
      getPaginationStatus() {
        return Utility.getPaginationStatus(this.total, this.pageIndex, this.pageSize,this.rows.length);
      }
    

    getReports(){
        this.reportService.getSurveyReports({
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
        const rows:SurveyReportTableData[]=[];
        for(let task of this.reports){
            for(let surveyTask of task.dailySurveyTasks){
                const base =  new SurveyReportTableData();
                
                if(task.cmUser) base.cmUserName = task.cmUser.name                
                base.displayDate = Utility.getDateToStringFormat(task.dateTimeStr);
                if(task.salesPoint) base.salesPointName = task.salesPoint.name;
                if(surveyTask.outlet) {
                    base.outletName = surveyTask.outlet.name;
                    base.outletCode = surveyTask.outlet.code;
                    if(surveyTask.outlet.route){
                        base.route = surveyTask.outlet.route.routeName;
                    }
                }

                if(!surveyTask.isOutletOpen){
                    ReportUtility.SetOutletClosedStatus(base);
                    rows.push(base);
                    continue;
                }
                else if(!surveyTask.isCompleted){
                    ReportUtility.SetIncompleteStatus(base,surveyTask);
                    rows.push(base);
                    continue;
                }
                
                for(let TaskAnswer of surveyTask.dailySurveyTaskAnswers){
                    const row = {...base};
                    if(task.cmUser) row.cmUserName = task.cmUser.name
                    row.answer = TaskAnswer.answer;                    
                    row.displayStatus = ReportConst.Comlete;
                    if(TaskAnswer.question) {
                        row.questionName = TaskAnswer.question.questionTitle;
                        if(TaskAnswer.question.questionType === "Signature") row.btnTitle = "View Signature";
                    }
                    if(surveyTask.surveyQuestionSet) row.surveyName = surveyTask.surveyQuestionSet.name;
                    rows.push(row);
                }
            }
        }
        this.rows = rows;
    }


    getAllReportData(pageIndex, pageSize, search) {
        this.alertService.fnLoading(true);
        this.cmTaskGenerationService.getSurveyReports(pageIndex, pageSize, search).subscribe(
            (res: any) => {
                this.surveyReportList = res.data.item1;
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


    public ptableSettings:IPTableSetting<colDef<keyof SurveyReportTableData>> = {
        tableID: "complete-report-table",
        tableClass: "table-responsive",
        tableName: 'Survey Report List',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'Date', width: '8%', internalName: 'displayDate', sort: true, type: "" },
            { headerName: 'CM User', width: '8%', internalName: 'cmUserName', sort: true, type: "" },
            { headerName: 'Sales Point', width: '8%', internalName: 'salesPointName', sort: true, type: "" },
            { headerName: 'Route', width: '8%', internalName: 'route', sort: true, type: "" },
            { headerName: 'Outlet', width: '8%', internalName: 'outletName', sort: true, type: "" },
            { headerName: 'Outlet Code', width: '8%', internalName: 'outletCode', sort: true, type: "" },
            { headerName: 'Survey Name', width: '10%', internalName: 'surveyName', sort: true, type: "" },
            { headerName: 'Question', width: '15%', internalName: 'questionName', sort: true, type: "" },
            { headerName: 'Answer', width: '10%', internalName: 'answer', sort: true, type: "buttonAndText" ,onClick: 'true'},
            { headerName: 'Status', width: '8%', internalName: 'displayStatus', sort: true, type: "" },
            { headerName: 'Reason', width: '9%', internalName: 'reason', sort: true, type: "" },

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
        this.reportService.exportSurveyReportToExcel({
          fromDateTime:this.fromDate,
          toDateTime:this.toDate,
          pageIndex:1,
          pageSize:Utility.MaximumWorkableInteger,
          search:""
        }).subscribe(data => {
            const fileName = `SurveyReport_${Utility.getDateTimeSuffixForExcelFilename()}.xlsx`;
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
