import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { CmTaskGenerationService } from 'src/app/Shared/Services/DailyActivity/cm-task-generation.service';
import { Paginator } from 'primeng/paginator';
import { ExcelService } from '../../../Shared/Modules/p-table/service/excel.service';
import { Utility } from 'src/app/Shared/utility';

@Component({
    selector: 'app-dcma-report-details-list',
    templateUrl: './dcma-report-details-list.component.html',
    styleUrls: ['./dcma-report-details-list.component.css']
})
export class DCMAReportInDetailsListComponent implements OnInit {

    tosterMsgError: string = "Something went wrong!";
    public DCMAReportInDetailsList: any[] = [];

    search: string = "";
    pageIndex = 1;
    pageSize = 10;
    total: number;
    @ViewChild("paging", { static: false }) paging: Paginator;

    constructor(private cmTaskGenerationService: CmTaskGenerationService,
        private router: Router,
        private excelService: ExcelService,
        private activatedRoute: ActivatedRoute,
        private alertService: AlertService) { }

    ngOnInit() {
        this.getAllReportData(this.pageIndex, this.pageSize, this.search);
    }

    public fnExeclDownload() {
        // this.getAllReportData(this.pageIndex, this.pageSize, this.search, true);
        this.cmTaskGenerationService.getDCMAReportsInDetails(1, Utility.MaximumWorkableInteger, "").subscribe(res => {                
            this.excelService.exportAsExcelFile(res.item1, this.ptableSettings);
        });            
    }

    getAllReportData(pageIndex, pageSize, search, isAll=false) {
        this.alertService.fnLoading(true);
        this.cmTaskGenerationService.getDCMAReportsInDetails(pageIndex, pageSize, search).subscribe(
            (res) => {
                this.DCMAReportInDetailsList = res.item1;
                // this.total = res.data.item2;
                this.total = res.item2;
                this.paging.totalRecords = this.total;
                this.paging.showCurrentPageReport = true;
                let columnDefs = [];
                if (this.DCMAReportInDetailsList.length > 0) {
                    let columnKeys = Object.keys(this.DCMAReportInDetailsList[0]);
                    columnKeys.forEach(col => {
                        if (col != 'DailyCMActivityId') {
                            let columnDef = { headerName: col, internalName: col, sort: false }
                            columnDefs.push(columnDef);
                        }
                    });
                    this.ptableSettings.tableColDef = columnDefs;        
                   
                }
                console.log(res);
            },
            (error) => {
                this.alertService.fnLoading(false);
                this.alertService.tosterDanger(this.tosterMsgError);
            },
            () => this.alertService.fnLoading(false));
    }

    fnSearch($event: any) {
        this.reset();
        this.search = $event.searchVal
        this.getAllReportData(this.pageIndex, this.pageSize, this.search);

    }
    paginate(event) {
        this.pageIndex = Number(event.page) + 1;
        this.pageSize = event.rows;
        this.getAllReportData(this.pageIndex, this.pageSize, this.search);
        this.ptableSettings.serverSitePageIndex = this.pageIndex;
    }
    reset() {
        this.paging.first = 1;
        this.pageIndex = 1;
        this.pageSize = 10;
    }
    public ptableSettings: IPTableSetting = {
        tableID: "dcma-report-details-table",
        tableClass: "table-responsive",
        tableName: 'DCMA In Details Reports',
        tableRowIDInternalName: "Id",
        tableColDef: [],
        enabledSearch: true,
        enabledSerialNo: true,
      
        enabledPagination: true,
        // enabledColumnFilter: true, 
        enabledExcelDownload: false,
        enabledServerSitePaggination: true,
        tableFooterVisibility: false
    };

    fnCustomTrigger(event) {

    }

}
