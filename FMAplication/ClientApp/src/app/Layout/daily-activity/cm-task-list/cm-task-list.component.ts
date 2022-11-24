import { Component, OnInit, ViewChild } from '@angular/core';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { DailyCMActivity } from 'src/app/Shared/Entity';
import { CmTaskGenerationService } from 'src/app/Shared/Services/DailyActivity/cm-task-generation.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { PermissionGroup, ActivityPermissionService } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalStatusChangeComponent } from '../modal-status-change/modal-status-change.component';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { Paginator } from 'primeng/paginator';
import { ExcelService } from '../../../Shared/Modules/p-table/service/excel.service';




@Component({
    selector: 'app-cm-task-list',
    templateUrl: './cm-task-list.component.html',
    styleUrls: ['./cm-task-list.component.css']
})
export class CmTaskListComponent implements OnInit {

    tosterMsgCntSuccess: string = "Status has been changed successfully.";
    tosterMsgError: string = "Something went wrong!";
    // public DailyCMAactivityList:DailyCMActivity[]=[];
    public DailyCMAactivityList: any[] = [];
    permissionGroup: PermissionGroup = new PermissionGroup();
    closeResult: string;
    statusType = StatusTypes.statusType;

    search: string = "";
    pageIndex = 1;
    pageSize = 20;
    total: number;
    @ViewChild("paging", { static: false }) paging: Paginator;
    showingPageDetails:any;

    constructor(private cmTaskGenerationService: CmTaskGenerationService,
        private router: Router,
        private activityPermissionService: ActivityPermissionService,
        private activatedRoute: ActivatedRoute,
        private modalService: NgbModal,
        private alertService: AlertService) {
        this.initPermissionGroup();
    }


    ngOnInit() {
        this.getAllCMTask(this.pageIndex, this.pageSize, this.search);

    }
  
 

    fnSearch($event: any) {
        this.reset();
        this.search = $event.searchVal;
        this.getAllCMTask(this.pageIndex, this.pageSize, this.search);

    }
    reset() {
        this.paging.first = 1;
        this.pageIndex = 1;
        this.pageSize = 10;
    }

    paginate(event) {
        this.pageIndex = Number(event.page) + 1;
        this.pageSize = event.rows;
        this.getAllCMTask(this.pageIndex, this.pageSize, this.search);
        this.generatedCmTaskPtableSettings.serverSitePageIndex = this.pageIndex;
    }
    
    private initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
    }


    getAllCMTask(pageIndex, pageSize, search) {
        this.alertService.fnLoading(true);
        this.cmTaskGenerationService.getDailyCMActivityList(pageIndex, pageSize, search).subscribe(
            (res: any) => {
                this.DailyCMAactivityList = res.data.item1;
                this.total = res.data.item2;
                this.paging.totalRecords = this.total;
                this.paging.showCurrentPageReport = true;
                
                //let totla = this.total - this.pageSize;
                //this.paging.currentPageReportTemplate = `showing ${totla} of ${this.total}`;
                console.log(res.data);
                //this.ptable.setPage1(this.pageIndex, this.total)

                // this.DailyCMAactivityList.forEach(s => {

                //   s.displayIsAudit = s.isAudit ? 'YES' : 'NO';
                //   s.displayIsPOSM = s.isPOSM ? 'YES' : 'NO';
                //   s.displayIsSurvey = s.isSurvey ? 'YES' : 'NO';
                //   s.displayIsConsSurAct = s.isConsumerSurveyActive ? 'YES' : 'NO';
                //   s.displayDate = s.dateStr;
                //   // s.displayStatus = this.statusType[s.status].label;

                //   const mapObject = this.statusType.filter(k => k.id == s.status)[0];

                //   if(mapObject != null)
                //   {
                //     s.displayStatus = mapObject.label;
                //   }

                //   if(s.assignedFMUser)
                //   {
                //     s.fmUserName = s.assignedFMUser.name;
                //   }

                //   if(s.outlet.route)
                //   {
                //     s.routeName = s.outlet.route.routeName;
                //   }

                //   if(s.cm)
                //   {
                //     s.cmUserName = s.cm.name;
                //   }

                //   if(s.outlet.salesPoint)
                //   {
                //     s.salesPointName = s.outlet.salesPoint.name;
                //   }

                //  });


            },
            (error) => {
                this.alertService.fnLoading(false);
                this.alertService.tosterDanger(this.tosterMsgError);
            },
            () => this.alertService.fnLoading(false));
    }





    public generatedCmTaskPtableSettings: IPTableSetting = {
        tableID: "generated-cm-task-table",
        tableClass: "table table-border ",
        tableName: '',
        tableRowIDInternalName: "id",
        tableColDef: [
            { headerName: 'Date', width: '8%', internalName: 'displayDate', sort: true, type: "" },
            { headerName: 'FM User ', width: '10%', internalName: 'fmUserName', sort: true, type: "" },
            { headerName: 'Route', width: '10%', internalName: 'routeName', sort: true, type: "" },
            { headerName: 'Sales Point', width: '10%', internalName: 'salesPointName', sort: true, type: "" },
            { headerName: 'Outlet', width: '10%', internalName: 'outletName', sort: true, type: "" },
            { headerName: 'CM User ', width: '10%', internalName: 'cmUserName', sort: true, type: "" },
            { headerName: 'Is POSM', width: '7%', internalName: 'displayIsPOSM', sort: true, type: "" },
            { headerName: 'Is Audit', width: '7%', internalName: 'displayIsAudit', sort: true, type: "" },
            { headerName: 'Is Survey', width: '7%', internalName: 'displayIsSurvey', sort: true, type: "" },
            { headerName: 'Is Consumer Survey', width: '10%', internalName: 'displayIsConsSurAct', sort: true, type: "" },
            { headerName: 'Status', width: '8%', internalName: 'displayStatus', sort: true, type: "" },

        ],
        enabledSearch: true,
        enabledSerialNo: true,
        pageSize: 10,
        enabledPagination: true,
        //enabledAutoScrolled:true,
        enabledCellClick: true,
        enabledColumnFilter: false,
        enabledEditBtn: false,
        enabledDeleteBtn: false,
        enabledServerSitePaggination: true,
        tableFooterVisibility: false,
       

    };


    public fnCustomTrigger(event) {

        if (event.action == "new-record") {
            this.createCMTask();
        }

    }

    createCMTask() {
        this.router.navigate(['/task/cm-task-generation']);
    }


    public fnChangeStatus(event) {
        if (this.permissionGroup.canUpdate) {
            if (event.cellName == 'displayStatus') {
                var postModel: DailyCMActivity = new DailyCMActivity();
                postModel.id = event.record.id;


                if (event.record.status == 0) {
                    this.alertService.confirm("Are you sure you want to change status into 'Active'?",
                        () => {
                            this.alertService.fnLoading(true);
                            postModel.status = 1;
                            this.cmTaskGenerationService.updateStatus(postModel).subscribe(
                                (succ: any) => {
                                    this.alertService.tosterSuccess(this.tosterMsgCntSuccess);
                                    this.getAllCMTask(this.pageIndex, this.pageSize, this.search);
                                },
                                (error) => {
                                    this.alertService.fnLoading(false);
                                    this.alertService.tosterDanger(this.tosterMsgError);
                                },
                                () => this.alertService.fnLoading(false));
                        }, () => { });

                }
                else {
                    this.alertService.confirm("Are you sure you want to change status into 'Inactive'?",
                        () => {
                            this.alertService.fnLoading(true);
                            postModel.status = 0;
                            this.cmTaskGenerationService.updateStatus(postModel).subscribe(
                                (succ: any) => {
                                    this.alertService.tosterSuccess(this.tosterMsgCntSuccess);
                                    this.getAllCMTask(this.pageIndex, this.pageSize, this.search);
                                },
                                (error) => {
                                    this.alertService.fnLoading(false);
                                    this.alertService.tosterDanger(this.tosterMsgError);
                                },
                                () => this.alertService.fnLoading(false));
                        }, () => { });
                }

            }

        }

    }
   
    openStatusChangeModal() {
        let ngbModalOptions: NgbModalOptions = {
            backdrop: 'static',
            keyboard: false
        };
        const modalRef = this.modalService.open(ModalStatusChangeComponent, ngbModalOptions);
        // modalRef.componentInstance.workflowLogModel = workflowLog;
        // modalRef.componentInstance.workflowStatus = status;

        modalRef.result.then((result) => {
            console.log(result);
            this.closeResult = `Closed with: ${result}`;
            this.getAllCMTask(this.pageIndex, this.pageSize, this.search);
        },
            (reason) => {
                console.log(reason);
            });
    }





}
