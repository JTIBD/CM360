import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { Survey, SurveyQuestionSet } from 'src/app/Shared/Entity';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { SurveyService } from 'src/app/Shared/Services/Question-Details/survey.service';
import * as moment from 'moment';
import { RoutesQuestionDetails } from 'src/app/Shared/Routes/RoutesQuestionDetails';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';

@Component({
  selector: 'app-edit-survey',
  templateUrl: './edit-survey.component.html',
  styleUrls: ['./edit-survey.component.css']
})
export class EditSurveyComponent implements OnInit {
  public form: FormGroup;
  survey:Survey;
  canEditStartDate=false;
  public canEditEndDate = true;
  public selectedFromDate: NgbDateStruct;
  public selectedToDate: NgbDateStruct;
  public minDate:NgbDateStruct;
  public enumStatusTypes: MapObject[] = StatusTypes.statusType.slice(0,2);
  public surveyStatus: number;

  questionSets:SurveyQuestionSet[]=[];
  selectedQuestionSetId:number;

  constructor(private router:Router,private route:ActivatedRoute,
    private surveyService:SurveyService,private alertService:AlertService,
    private commonService:CommonService) { }

  ngOnInit() {
    this.minDate = this.commonService.dateToNgbDate(new Date());
    if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
      // console.log("id", this.route.snapshot.params.id);
      let surveyId = this.route.snapshot.params.id;
      this.surveyService.getSurveyById(surveyId).subscribe(res=>{
        this.survey = res;
        if(new Date() < new Date(this.survey.fromDateStr) ) this.canEditStartDate = true;
        if (new Date() > new Date(this.survey.toDateStr) ) this.canEditEndDate = false;
        this.selectedQuestionSetId = this.survey.surveyQuestionSetId;
        this.selectedFromDate = this.commonService.dateToNgbDate(new Date(this.survey.fromDateStr));
        this.selectedToDate = this.commonService.dateToNgbDate(new Date(this.survey.toDateStr));
        this.surveyStatus = this.survey.status;
      });
    }
    else this.alertService.tosterDanger("survey id not found");
    this.surveyService.getAllQuestionSet().subscribe((res:any)=>{
      console.log(res.data);
      this.questionSets = res.data;
    })
  }
  submit(){
    let isUpated = false;
    let toDateObj = this.commonService.ngbDateToDate(this.selectedToDate);
    toDateObj.setHours(23,59,59);
    let toDate = toDateObj.toISOString();

    if(!moment(this.survey.toDateStr).isSame(toDate,"second")) {
      isUpated = true;
      this.survey.toDate = toDate;
    }
    if(this.canEditStartDate){
      let fromDateObj = this.commonService.ngbDateToDate(this.selectedFromDate);      
      let fromDate = fromDateObj.toISOString();
      if(!moment(this.survey.fromDateStr).isSame(fromDate,"second")){
        isUpated = true;
        this.survey.fromDate = fromDate;
      }
      if(this.survey.surveyQuestionSetId !== this.selectedQuestionSetId){
        isUpated = true;
        this.survey.surveyQuestionSetId = this.selectedQuestionSetId;
      }
    }
    if (this.survey.status != this.surveyStatus) {
      this.survey.status = this.surveyStatus;
      isUpated = true;
    }

    if(!isUpated){
      this.alertService.tosterDanger("Nothing to update");
      return;
    }

    let update=()=>{
      this.surveyService.editSurvey(this.survey).subscribe(res=>{
        this.alertService.tosterSuccess("Successfully updated survey");
        this.router.navigate(["/question/survey-setup"]);
      });
    }

    if (this.survey.status == 0) {
      this.alertService.confirm(`InActive setup can't be reverted to active. Are your sure to make the setup inactive?`,
        () => { update(); },
        () => {}
      );
      return;
    }

    this.surveyService.getExistingSurvey({data:[this.survey]}).subscribe(res=>{
      res = res.filter(x=>x.id != this.survey.id);
      if(res.length) {
        if(this.alertService.loadingFlag) this.alertService.fnLoading(false);        

        this.alertService.confirm(`Survey already exist in Salespoint ${this.survey.salesPoint.name}. Do you want to stop the surveys before the new survey starts?`, () => {
          update();
        },()=>{}); 

      }
      else update();      
    })
    
  }

  handleBack(){
    this.router.navigate([RoutesQuestionDetails.parent,RoutesQuestionDetails.surveySetup]);
  }


}
