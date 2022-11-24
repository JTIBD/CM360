import { Component, OnInit } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { first } from 'rxjs/operators';
import { NodeTree, SalesPoint, Survey, SurveyQuestionSet } from 'src/app/Shared/Entity';
import { TaskAssignedUserType, TaskAssignedUserTypeStrs } from 'src/app/Shared/Enums';
import { IDateRange } from 'src/app/Shared/interfaces';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { SurveyService } from 'src/app/Shared/Services/Question-Details/survey.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { Utility } from 'src/app/Shared/utility';
import * as moment from 'moment';
import { Router } from '@angular/router';
import { RoutesQuestionDetails } from 'src/app/Shared/Routes/RoutesQuestionDetails';

@Component({
  selector: 'app-new-survey',
  templateUrl: './new-survey.component.html',
  styleUrls: ['./new-survey.component.css']
})
export class NewSurveyComponent implements OnInit {

  public selectedFromDate: NgbDateStruct;
  public selectedToDate: NgbDateStruct;
  public minDate:NgbDateStruct;
  selectedQuestionSetId:number;
  selectedUserType:TaskAssignedUserType= TaskAssignedUserType.BOTH;
  userTypes = TaskAssignedUserTypeStrs;
  questionSets:SurveyQuestionSet[]=[];
  
  isConsumerSurvey=false;

  nodeTree:NodeTree[]=[];

  salesPointIds:number[]=[]

  
  constructor(private commonService:CommonService,
    private surveyService:SurveyService,
    private userService:UserService,
    private alertService:AlertService,
    private router: Router) { }

  ngOnInit() {
    this.minDate = this.commonService.dateToNgbDate(new Date());
    this.surveyService.getAllQuestionSet().subscribe((res:any)=>{
      this.questionSets = res.data;
    })

    this.userService.getNodeTreeByCurrentUser().subscribe(data=>{
      this.nodeTree = data;
    })
  }

  handleFromDateChange(){
    let from = this.commonService.ngbDateToDate(this.selectedFromDate).toISOString();
    let toDate = this.commonService.ngbDateToDate(this.selectedToDate).toISOString();
    if(moment(from).isAfter(toDate,"date") ) {
      this.selectedToDate = null;
    }
  }
  handleToDateChange(){    
    let from = this.commonService.ngbDateToDate(this.selectedFromDate).toISOString();
    let toDate = this.commonService.ngbDateToDate(this.selectedToDate).toISOString();
    if(moment(from).isAfter(toDate,"date") ) {
      this.selectedFromDate = null;
    }
  }
  getSelectedSalesPoints(){
    const salesPointIds:SalesPoint[]=[];
    let fun=(tree:NodeTree[])=>{
      tree.forEach(tr=>{
        if(!!tr.salesPoints && !!tr.salesPoints.length){
          tr.salesPoints.forEach(x=>{
            if(x.isSelected) salesPointIds.push(x);
          })
        }
        else if(!!tr.nodes) fun(tr.nodes);
      })
    }
    fun(this.nodeTree);
    return  salesPointIds;
  }
  
  submit(){
    const surveyList:Survey[]=[];
    const fromDateStr = this.commonService.ngbDateToDate(this.selectedFromDate).toISOString();
    const toDateStr = this.commonService.ngbDateToDate(this.selectedToDate).toISOString();
    const dateRange:IDateRange={
      from:fromDateStr,
      to:toDateStr,
    };
    Utility.adjustDateRange(dateRange);
    const salesPoints = this.getSelectedSalesPoints();
    if(!salesPoints.length) {
      this.alertService.tosterDanger("No salespoint selected");
      return;
    }
    salesPoints.forEach(sp=>{
      const survey = new Survey();
      survey.code="SV_"+sp.code;
      survey.fromDate=dateRange.from;
      survey.toDate = dateRange.to;
      survey.salesPointId = sp.salesPointId;
      survey.surveyQuestionSetId = this.selectedQuestionSetId;
      survey.userType = this.selectedUserType;
      survey.isConsumerSurvey=this.isConsumerSurvey;
      surveyList.push(survey);
    });
    this.surveyService.getExistingSurvey({data:surveyList}).pipe(first()).subscribe(res=>{
      if(res.length) {
        if(this.alertService.loadingFlag) this.alertService.fnLoading(false);
        let sps = salesPoints.filter(x=> res.some(sv=>sv.salesPointId == x.salesPointId)).filter((sp,i,arr)=> arr.findIndex(x=>x.salesPointId == sp.salesPointId) === i);
        let spNames = sps.map(x=>x.name);

        this.alertService.confirm(`Survey already exist in Salespoint ${spNames.join(", ")}. Do you want to stop the surveys before the new survey starts?`, () => {
          this.surveyService.createNewSurvey({ data: surveyList }).subscribe(res => {
            this.alertService.tosterSuccess("Surveys created successfully");
            this.router.navigate(["/question/survey-setup"]);
          });
        },()=>{}); 

      }
      else{
        this.surveyService.createNewSurvey({ data: surveyList }).subscribe(res => {
          console.log(res);
          this.alertService.tosterSuccess("Surveys created successfully");
          this.router.navigate(["/question/survey-setup"]);
        });
      }
    })

  }
  handleSalesPointSelect(event,salesPoint:SalesPoint){
    salesPoint.isSelected = event.target.checked;    
  }

  handleNodeSelect(item:NodeTree,checked:boolean){
    console.log(item,checked);
    const node:NodeTree = this.getNodeById(item.node.id);
    console.log(node);
    if(!node) return;
    let fun = (trees:NodeTree[],checked:boolean)=>{
      trees.forEach(tr=>{
        tr.isSelected = checked;
        if(!!tr.nodes && tr.nodes.length) fun(tr.nodes,checked);
        else if(!!tr.salesPoints && !!tr.salesPoints.length){
          tr.salesPoints.forEach(sl=>{
            sl.isSelected = checked;
          })
        }
      })
    }
    fun([node],checked);
  }

  getNodeById(id:number){
    let find=(tree:NodeTree[])=>{
      let node = tree.find(t=>t.node.id === id);
      if(node) return node;
      //@ts-ignore
      else return find(tree.filter(x=>!!x.nodes).map(x=>x.nodes).flat());
    }
    return find(this.nodeTree);
  
  }

  hanleSelect(){    
    this.selectedUserType = TaskAssignedUserType.BOTH;
  }

  handleBack(){
    this.router.navigate([RoutesQuestionDetails.parent,RoutesQuestionDetails.surveySetup]);
  }


}

