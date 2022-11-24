import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AvCommunication, CampaignType } from 'src/app/Shared/Entity/AVCommunications/avCommunication';
import { AvSetup } from 'src/app/Shared/Entity/AVCommunications/avSetup';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import {AvCommunicationService } from 'src/app/Shared/Services/AvCommunication/avCommunication.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import * as moment from 'moment';
import { RoutesAvCommunication } from 'src/app/Shared/Routes/RoutesAvCommunication';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';

@Component({
  selector: 'app-edit-av-setup',
  templateUrl: './edit-av-setup.component.html',
  styleUrls: ['./edit-av-setup.component.css']
})
export class EditAvSetupComponent implements OnInit {

  public form: FormGroup;
  avSetup:AvSetup;
  canEditStartDate=false;
  public canEditEndDate = true;
  public selectedFromDate: NgbDateStruct;
  public selectedToDate: NgbDateStruct;
  public minDate:NgbDateStruct;
  public enumStatusTypes: MapObject[] = StatusTypes.statusType.slice(0,2);
  public avStatus: number;

  avSets:AvCommunication[]=[];
  selectedAVId:number;

  constructor(private router:Router,private route:ActivatedRoute,
    private avService:AvCommunicationService,private alertService:AlertService,
    private commonService:CommonService) { }

  ngOnInit() {
    this.minDate = this.commonService.dateToNgbDate(new Date());
    if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
      // console.log("id", this.route.snapshot.params.id);
      let avSetupId = this.route.snapshot.params.id;
      this.avService.getAvSetupById(avSetupId).subscribe(res=>{
        this.avSetup = res;
        if(new Date() < new Date(this.avSetup.fromDateStr) ) this.canEditStartDate = true;
        if (new Date() > new Date(this.avSetup.toDateStr) ) this.canEditEndDate = false;
        this.selectedAVId = this.avSetup.avId;
        this.selectedFromDate = this.commonService.dateToNgbDate(new Date(this.avSetup.fromDateStr));
        this.selectedToDate = this.commonService.dateToNgbDate(new Date(this.avSetup.toDateStr));
        this.avStatus = this.avSetup.status;
      });
    }
    else this.alertService.tosterDanger("avSetup id not found");
    this.avService.getAll().subscribe((res)=>{
      console.log(res);
      this.avSets = res.filter(x=>x.campaignType === CampaignType.Video);
    })
  }
  submit(){
    let isUpated = false;
    let toDateObj = this.commonService.ngbDateToDate(this.selectedToDate);
    toDateObj.setHours(23,59,59);
    let toDate = toDateObj.toISOString();

    if(!moment(this.avSetup.toDateStr).isSame(toDate,"second")) {
      isUpated = true;
      this.avSetup.toDate = toDate;
    }
    if(this.canEditStartDate){
      let fromDateObj = this.commonService.ngbDateToDate(this.selectedFromDate);      
      let fromDate = fromDateObj.toISOString();
      if(!moment(this.avSetup.fromDateStr).isSame(fromDate,"second")){
        isUpated = true;
        this.avSetup.fromDate = fromDate;
      }
      if(this.avSetup.avId !== this.selectedAVId){
        isUpated = true;
        this.avSetup.avId = this.selectedAVId;
      }
    }
    if (this.avSetup.status != this.avStatus) {
      this.avSetup.status = this.avStatus;
      isUpated = true;
    }

    if(!isUpated){
      this.alertService.tosterDanger("Nothing to update");
      return;
    }

    let update=()=>{
      this.avService.editAvSetup(this.avSetup).subscribe(res=>{
        this.alertService.tosterSuccess("Successfully updated avSetup");
        this.router.navigate(["/av-communication/av-setups"]);
      });
    }

    if (this.avSetup.status == 0) {
      this.alertService.confirm(`InActive setup can't be reverted to active. Are your sure to make the setup inactive?`,
        () => { update(); },
        () => {}
      );
      return;
    }

    this.avService.getExistingAvSetup({data:[this.avSetup]}).subscribe(res=>{
      res = res.filter(x=>x.id !== this.avSetup.id)
      if(res.length) {
        if(this.alertService.loadingFlag) this.alertService.fnLoading(false);        

        this.alertService.confirm(`AvSetup already exist in Salespoint ${this.avSetup.salesPoint.name}. Do you want to stop the avSetups before the new avSetup starts?`, () => {
          update();
        },()=>{}); 

      }
      else update();      
    })
    
  }

  handleBack(){
    this.router.navigate([RoutesAvCommunication.Parent,RoutesAvCommunication.AvSetup]);
  }



}
