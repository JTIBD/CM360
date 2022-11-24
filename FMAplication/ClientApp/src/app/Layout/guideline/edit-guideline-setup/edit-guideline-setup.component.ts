import { AlertService } from './../../../Shared/Modules/alert/alert.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { GuidelineSetupService } from './../../../Shared/Services/GuidelineSetup/guidelineSetup.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Route } from './../../../Shared/Entity/Sales/route';
import { CommonService } from './../../../Shared/Services/Common/common.service';
import { TaskAssignedUserType, TaskAssignedUserTypeStrs } from './../../../Shared/Enums/surveyAssignedUserType';
import { Component, OnInit } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { GuidelineSetup } from 'src/app/Shared/Entity/Guidelines/guideline-setup';
import * as moment from 'moment';
import { RoutesGuidelineModule } from 'src/app/Shared/Routes/RoutesGuidelineModule';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';

@Component({
  selector: 'app-edit-guideline-setup',
  templateUrl: './edit-guideline-setup.component.html',
  styleUrls: ['./edit-guideline-setup.component.css']
})
export class EditGuidelineSetupComponent implements OnInit {

  guidelineSetup: GuidelineSetup;
  public selectedFromDate: NgbDateStruct;
  public selectedToDate: NgbDateStruct;
  public minDate:NgbDateStruct;
  canEditStartDate = false;
  public canEditEndDate = true;
  public enumStatusTypes: MapObject[] = StatusTypes.statusType.slice(0,2);
  public guidelineStatus: number;

  htmlContent = '';
  config: AngularEditorConfig;

  constructor(private commonService:CommonService, private route:ActivatedRoute,
    private guidelineSetupService: GuidelineSetupService, private alertService:AlertService,
    private router: Router){

  }

  ngOnInit(){
    this.minDate = this.commonService.dateToNgbDate(new Date());
    this.richTextBoxConfig();
    if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
      let id = this.route.snapshot.params.id;
      this.guidelineSetupService.getById(id).subscribe(res=>{
        this.guidelineSetup = res.data;
        this.htmlContent = this.guidelineSetup.guidelineText;
        
        if(new Date() < new Date(this.guidelineSetup.fromDateStr) ) this.canEditStartDate = true;
        if (new Date() > new Date(this.guidelineSetup.toDateStr) ) this.canEditEndDate = false;

        this.selectedFromDate = this.commonService.dateToNgbDate(new Date(this.guidelineSetup.fromDateStr));
        this.selectedToDate = this.commonService.dateToNgbDate(new Date(this.guidelineSetup.toDateStr));
        this.guidelineStatus = this.guidelineSetup.status;
      });
    }
  }
  submit(){
    let isUpdated = false;
    let toDateObj = this.commonService.ngbDateToDate(this.selectedToDate);
    toDateObj.setHours(23,59,59);
    let toDate = toDateObj.toISOString();

    if(!moment(this.guidelineSetup.toDateStr).isSame(toDate,"second")) {
      isUpdated = true;
      this.guidelineSetup.toDate = toDate;
    }
    if(this.canEditStartDate){
      let fromDateObj = this.commonService.ngbDateToDate(this.selectedFromDate);      
      let fromDate = fromDateObj.toISOString();
      if(!moment(this.guidelineSetup.fromDateStr).isSame(fromDate,"second")){
        isUpdated = true;
        this.guidelineSetup.fromDate = fromDate;
      }
    }
    if(!moment(this.guidelineSetup.guidelineText).isSame(this.htmlContent)){
      isUpdated = true;
      this.guidelineSetup.guidelineText = this.htmlContent;
    }
    if (this.guidelineSetup.status != this.guidelineStatus) {
      this.guidelineSetup.status = this.guidelineStatus;
      isUpdated = true;
    }

    if(!isUpdated){
      this.alertService.tosterDanger("Nothing to update");
      return;
    }

    let update=()=>{
      this.guidelineSetupService.updateGuidelineSetup(this.guidelineSetup).subscribe(res=>{
        this.alertService.tosterSuccess("Successfully updated Guideline Setup");
        this.router.navigate(["/guideline/guideline-setup"]);
      });
    }

    if (this.guidelineSetup.status == 0) {
      this.alertService.confirm(`InActive setup can't be reverted to active. Are your sure to make the setup inactive?`,
        () => { update(); },
        () => {}
      );
      return;
    }

    this.guidelineSetupService.getExistingGuidelineSetup({data:[this.guidelineSetup]}).subscribe(res=>{
      res = res.filter(x=>x.id !== this.guidelineSetup.id)
      if(res.length) {
        if(this.alertService.loadingFlag) this.alertService.fnLoading(false);        

        this.alertService.confirm(`Guideline setup already exist in Salespoint ${this.guidelineSetup.salesPoint.name}. Do you want to stop the Guideline setups before the new guideline Setup starts?`, () => {
          update();
        },()=>{}); 

      }
      else update();      
    })
    
  }

  handleBack(){
    this.router.navigate([RoutesGuidelineModule.Parent,RoutesGuidelineModule.GuidelineSetup]);
  }

  richTextBoxConfig() {
    this.config = {
      editable: true,
      minHeight: '10rem',
      spellcheck: true,
      translate: 'yes',
      enableToolbar: true,
      showToolbar: true,
      placeholder: 'Enter text here...',
      defaultParagraphSeparator: '',
      defaultFontName: '',
      defaultFontSize: '',
      fonts: [
        { class: 'arial', name: 'Arial' },
        { class: 'times-new-roman', name: 'Times New Roman' },
        { class: 'calibri', name: 'Calibri' },
        { class: 'comic-sans-ms', name: 'Comic Sans MS' }
      ],
      uploadWithCredentials: false,
      sanitize: true,
      toolbarPosition: 'top',
      toolbarHiddenButtons: [
        [
          // 'undo',
          // 'redo',
          // 'bold',
          // 'italic',
          // 'underline',
          // 'strikeThrough',
          // 'subscript',
          // 'superscript',
          // 'justifyLeft',
          // 'justifyCenter',
          // 'justifyRight',
          // 'justifyFull',
          // 'indent',
          // 'outdent',
          // 'insertUnorderedList',
          // 'insertOrderedList',
          // 'heading',
          // 'fontName'
        ],
        [
          // 'fontSize',
          // 'textColor',
          // 'backgroundColor',
          'customClasses',
          'link',
          'unlink',
          'insertImage',
          'insertVideo',
          'insertHorizontalRule',
          'removeFormat',
          'toggleEditorMode'
        ]
      ]
    };
  }
}
