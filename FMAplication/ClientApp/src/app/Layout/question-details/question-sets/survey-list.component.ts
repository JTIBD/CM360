import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SurveyQuestionSet } from 'src/app/Shared/Entity/Questions/survey';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { Status } from 'src/app/Shared/Enums/status';
import { SurveyService } from 'src/app/Shared/Services/Question-Details/survey.service';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { SurveyQuestionSetTableData } from 'src/app/Shared/Entity/Questions';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { RoutesQuestionDetails } from 'src/app/Shared/Routes/RoutesQuestionDetails';

@Component({
  selector: 'app-survey-list',
  templateUrl: './survey-list.component.html',
  styleUrls: ['./survey-list.component.sass']
})
export class SurveyListComponent implements OnInit {
  heading = 'Create A New Survey';
  subheading = 'Create new survey with multiple questions';
  icon = 'pe-7s-drawer icon-gradient bg-happy-itmeo';
  public enumStatus = Status;
  public tosterMsgDltSuccess: string = "Record Has been deleted successfully.";
  public tosterMsgError: string = "Something went wrong!";

  permissionGroup: PermissionGroup = new PermissionGroup();

  constructor(
    private router: Router,
    private surveyService: SurveyService,
    private activityPermissionService: ActivityPermissionService,
    private activatedRoute: ActivatedRoute,
    private alertService: AlertService
  ) { 
    this.initPermissionGroup();
  }

  public questionSets:SurveyQuestionSet[]=[];

  public questionSetTableData: SurveyQuestionSetTableData[] = [];

  ngOnInit() {
    this.getAllSuevey();
  }


  private initPermissionGroup() {
    this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
    this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
    this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
    this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
}

  createNewSurvey() {
    this.router.navigate([RoutesLaout.Question,RoutesQuestionDetails.questionSetGeneration]);
  }

  getAllSuevey() {
    this.alertService.fnLoading(true);
    this.surveyService.getAllQuestionSet().subscribe(
      (res: any) => {
        this.questionSets = res.data;
        this.questionSets.forEach(s => s.statusText = this.enumStatus[s.status]);
        this.mapTableData();
      },
      (error) => {
        console.log(error);
        this.alertService.fnLoading(false);
        this.alertService.tosterDanger(this.tosterMsgError);
      },
      () => this.alertService.fnLoading(false));
  }

  mapTableData(){
    const tableData:SurveyQuestionSetTableData[]=[];
    this.questionSets.forEach(set=>{
        let data = set as SurveyQuestionSetTableData;
        data.disableEdit = !set.isEditable;
        data.disableDelete = !set.isDeletable;
        data.disableView = set.isEditable;
        tableData.push(data);
    })
    this.questionSetTableData = tableData;
  }

  edit(id: number) {
    this.router.navigate([RoutesLaout.Question,RoutesQuestionDetails.questionSetGeneration,id]);    
  }

  delete(id: number) {
    this.alertService.confirm("Are you sure?",
      () => {
        this.alertService.fnLoading(true);
        this.surveyService.deleteSurvey(id).subscribe(
          (succ: any) => {
            console.log(succ.data);
            this.alertService.tosterSuccess(this.tosterMsgDltSuccess);
            this.getAllSuevey();
          },
          (error) => {
            this.alertService.fnLoading(false);
          },
          () => this.alertService.fnLoading(false));
      }, () => { });
  }

  public ptableSettings = {
    tableID: "Survey-table",
    tableClass: "table-responsive",
    tableName: 'Question sets',
    tableRowIDInternalName: "Id",
    tableColDef: [
      { headerName: 'Survey', width: '60%', internalName: 'name', sort: true, type: "" },
      { headerName: 'Status', width: '40%', internalName: 'statusText', sort: true, type: "" }
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 10,
    enabledPagination: true,
    enabledEditBtn: true,
	  enabledDeleteBtn: true,
    enabledColumnFilter: true,
    enabledRecordCreateBtn: true,
    enabledViewBtn:true,
  };

  public fnCustomTrigger(event) {
    console.log("custom  click: ", event);

    if (event.action == "new-record") {
      this.createNewSurvey();
    }
    else if (event.action == "edit-item") {
      this.edit(event.record.id);
    }
    else if (event.action == "delete-item") {
      this.delete(event.record.id);
    }
    else if(event.action = "view-item"){
      this.router.navigate([RoutesLaout.Question,RoutesQuestionDetails.questionSetGeneration,event.record.id]);
    }
  }
}


