import { Component, OnInit } from '@angular/core';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CmTaskGenerationService } from 'src/app/Shared/Services/DailyActivity/cm-task-generation.service';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { DailyCMActivity } from 'src/app/Shared/Entity';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { DailyCmTaskReport } from 'src/app/Shared/Entity/Daily-Activity/daily-cm-task-report';

@Component({
  selector: 'app-complete-report',
  templateUrl: './complete-report.component.html',
  styleUrls: ['./complete-report.component.css']
})
export class CompleteReportComponent implements OnInit {

  tosterMsgError: string = "Something went wrong!";
  public DailyCMAactivityList:DailyCmTaskReport[]=[];
  mapObject : MapObject;
  enumStatusTypes : MapObject[] = StatusTypes.statusType;

  constructor(private cmTaskGenerationService: CmTaskGenerationService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private alertService: AlertService) { }

  ngOnInit() {
    this.getAllReportData();
  }


  getAllReportData() {
    this.alertService.fnLoading(true);
    this.cmTaskGenerationService.getAllReportData().subscribe(
      (res: any) => {
        this.DailyCMAactivityList = res.data;
        console.log(res.data);
        
        
      },
      (error) => {
        this.alertService.fnLoading(false);
        this.alertService.tosterDanger(this.tosterMsgError);
      },
      () => this.alertService.fnLoading(false));
  }


  public ptableSettings = {
    tableID: "complete-report-table",
    tableClass: "table-responsive",
    tableName: 'Complete Report List',
    tableRowIDInternalName: "Id",
    tableColDef: [
        { headerName: 'Date', width: '8%', internalName: 'displayDate', sort: true, type: "" },
          { headerName: 'FM User ', width: '12%', internalName: 'fmUserName', sort: true, type: "" },
          { headerName: 'CM User ', width: '12%', internalName: 'cmUserName', sort: true, type: "" },
          { headerName: 'Outlet', width: '40%', internalName: 'outletNames', sort: true, type: "" },
          
        { headerName: 'POSM', width: '9%', internalName: 'posmPercentage', sort: true, type: "" },
        { headerName: 'Audit', width: '9%', internalName: 'auditPercentage', sort: true, type: "" },
        { headerName: 'Survey', width: '10%', internalName: 'surveyPercentage', sort: true, type: "" },
      //{ headerName: 'Is Consumer Survey', width: '10%', internalName: 'displayIsConsSurAct', sort: true, type: "" },
         
    
        ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 10,
    enabledPagination: true,
    enabledColumnFilter: true, 
    enabledRecordCreateBtn: false,
  };

  fnCustomTrigger(event){

  }



}
