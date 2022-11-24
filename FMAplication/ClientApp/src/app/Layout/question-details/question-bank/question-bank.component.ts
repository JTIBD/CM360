import { Component, OnInit } from '@angular/core';
import { QuestionService } from '../../../Shared/Services/Question-Details/question.service';
import { Question } from '../../../Shared/Entity/Questions/question';
import { Router, ActivatedRoute } from '@angular/router';
import { Status } from 'src/app/Shared/Enums/status';
import { QuestionTypes } from 'src/app/Shared/Enums/questionTypes';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { PermissionGroup, ActivityPermissionService } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { QuestionBankTableData } from 'src/app/Shared/Entity';

@Component({
  selector: 'app-question-bank',
  templateUrl: './question-bank.component.html',
  styleUrls: ['./question-bank.component.sass']
})
export class QuestionBankComponent implements OnInit {

  heading = 'Question Bank';
  subheading = '';
  icon = 'pe-7s-drawer icon-gradient bg-happy-itmeo';

  questionTypes = {};
  questions: Question[] = [];
  questionBankTableData: QuestionBankTableData[] = [];
  statusOptions = {};
  permissionGroup: PermissionGroup = new PermissionGroup();

  constructor(
    private router: Router,
    private questionService: QuestionService,
    private activityPermissionService: ActivityPermissionService,
    private activatedRoute: ActivatedRoute,

    private alertService: AlertService
  ) { 
    this.initPermissionGroup();
  }

  ngOnInit() {
    this.getAllQuestion();
  }

  private initPermissionGroup() {
    this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
    this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
    this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
    this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
}

  getAllQuestion() {
    this.alertService.fnLoading(true);
    this.statusOptions = Status;
    console.log("Status Options: ", this.statusOptions);
    this.questionTypes = QuestionTypes;
    //let keys = Object.keys(this.statusOptions).filter(k => !isNaN(Number(k)));

    this.questionService.getQuestionData().subscribe(
      (res: any) => {
        //console.log(res);
        let questionsData = res.data;
             
        questionsData.forEach(obj => {
          obj.statusText = this.statusOptions[obj.status];
          obj.questionType = this.questionTypes[obj.questionType];
        });
        //console.log(questionsData);
        this.questions = questionsData;
        this.mapToTableData();
      },
      (error) => {
        console.log(error);
      }, 
      () => this.alertService.fnLoading(false)
    );
  }

  mapToTableData(){
    let tableData:QuestionBankTableData[]=[];
    this.questions.forEach(q=>{
      const data = q as QuestionBankTableData;
      data.disableEdit = data.disableDelete = !q.isEditable;
      tableData.push(data);
    });
    this.questionBankTableData = tableData;
  }

  createNewQuestion() {
    this.router.navigate(['/question/save-question']);
  }

  edit(id: number) {
      this.router.navigate(['/question/save-question/' + id]);
  }

  delete(id: number) {
    // console.log("Id:", id);
    this.alertService.confirm("Are you sure you want to delete this item?", () => {
      this.questionService.deleteQuestion(id).subscribe(
        (res: any) => {
          // console.log(res.data);          
          this.alertService.tosterSuccess("Record has been deleted successfully.");
          this.getAllQuestion();
        },
        (error) => {
          console.log(error);
        }
      );
    }, () => {

    });
  }

  public fnPtableCellClick(event) {
    console.log("cell click: ");
  }

  public ptableSettings = {
    tableID: "Questions-table",
    tableClass: "table table-border ",
    tableName: 'Questions',
    tableRowIDInternalName: "Id",
    tableColDef: [
      { headerName: 'Question ', width: '60%', internalName: 'questionTitle', sort: true, type: "" },
      { headerName: 'Question Type ', width: '20%', internalName: 'questionType', sort: true, type: "" },
      { headerName: 'Status', width: '20%', internalName: 'statusText', sort: true, type: "" },
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 10,
    enabledPagination: true,
    //enabledAutoScrolled:true,
    enabledEditBtn: true,
	  enabledDeleteBtn: true,
    // enabledCellClick: true,
    enabledColumnFilter: true,
    // enabledDataLength:true,
    // enabledColumnResize:true,
    // enabledReflow:true,
    // enabledPdfDownload:true,
    // enabledExcelDownload:true,
    // enabledPrint:true,
    // enabledColumnSetting:true,
    enabledRecordCreateBtn: true,
    // enabledTotal:true,
  };

  public fnCustomTrigger(event) {
    console.log("custom  click: ", event);

    if (event.action == "new-record") {
      this.createNewQuestion();
    }
    else if (event.action == "edit-item") {
      this.edit(event.record.id);
    }
    else if (event.action == "delete-item") {
      this.delete(event.record.id);
    }
  }


}
