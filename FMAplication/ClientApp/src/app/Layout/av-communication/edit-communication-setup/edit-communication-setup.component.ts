import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AvCommunication, CampaignType } from 'src/app/Shared/Entity/AVCommunications/avCommunication';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { AvCommunicationService } from 'src/app/Shared/Services/AvCommunication/avCommunication.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import * as moment from 'moment';
import { CommunicationSetup } from 'src/app/Shared/Entity/AVCommunications/communicationSetup';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { RoutesAvCommunication } from 'src/app/Shared/Routes/RoutesAvCommunication';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';

@Component({
  selector: 'app-edit-communication-setup',
  templateUrl: './edit-communication-setup.component.html',
  styleUrls: ['./edit-communication-setup.component.css']
})
export class EditCommunicationSetupComponent implements OnInit {

  public form: FormGroup;
  communicationSetup:CommunicationSetup;
  canEditStartDate=false;
  canEditEndDate = true;
  public selectedFromDate: NgbDateStruct;
  public selectedToDate: NgbDateStruct;
  public minDate:NgbDateStruct;
  public enumStatusTypes: MapObject[] = StatusTypes.statusType.slice(0,2);
  public communicationStatus: number;


  avSets:AvCommunication[]=[];
  selectedAVId:number;
//private avService:AvCommunicationService
  constructor(private router:Router,private route:ActivatedRoute,
    private communicationService:AvCommunicationService,private alertService:AlertService,
    private commonService:CommonService) { }

  ngOnInit() {
    this.minDate = this.commonService.dateToNgbDate(new Date());
    if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
      // console.log("id", this.route.snapshot.params.id);
      let commSetupId = this.route.snapshot.params.id;
      this.communicationService.getCommunicationSetupById(commSetupId).subscribe(res=>{
        this.communicationSetup = res;
        if(new Date() < new Date(this.communicationSetup.fromDateStr) ) this.canEditStartDate = true;
        if (new Date() > new Date(this.communicationSetup.toDateStr) ) this.canEditEndDate = false;
        this.selectedAVId = this.communicationSetup.avCommunicationId;
        this.selectedFromDate = this.commonService.dateToNgbDate(new Date(this.communicationSetup.fromDateStr));
        this.selectedToDate = this.commonService.dateToNgbDate(new Date(this.communicationSetup.toDateStr));
        this.communicationStatus = this.communicationSetup.status;
      });
    }
    else this.alertService.tosterDanger("communicationSetup id not found");
    this.communicationService.getAll().subscribe((res)=>{
      console.log(res);
      this.avSets = res.filter(x=>x.campaignType === CampaignType.Image);
    })
  }
  submit(){
    let isUpdated = false;
    let toDateObj = this.commonService.ngbDateToDate(this.selectedToDate);
    toDateObj.setHours(23,59,59);
    let toDate = toDateObj.toISOString();

    if(!moment(this.communicationSetup.toDateStr).isSame(toDate,"second")) {
      isUpdated = true;
      this.communicationSetup.toDate = toDate;
    }
    if(this.canEditStartDate){
      let fromDateObj = this.commonService.ngbDateToDate(this.selectedFromDate);      
      let fromDate = fromDateObj.toISOString();
      if(!moment(this.communicationSetup.fromDateStr).isSame(fromDate,"second")){
        isUpdated = true;
        this.communicationSetup.fromDate = fromDate;
      }
      if(this.communicationSetup.avCommunicationId !== this.selectedAVId){
        isUpdated = true;
        this.communicationSetup.avCommunicationId = this.selectedAVId;
      }
    }
    if (this.communicationSetup.status != this.communicationStatus) {
      this.communicationSetup.status = this.communicationStatus;
      isUpdated = true;
    }

    if(!isUpdated){
      this.alertService.tosterDanger("Nothing to update");
      return;
    }

    let update=()=>{
      this.communicationService.editCommunicationSetup(this.communicationSetup).subscribe(res=>{
        this.alertService.tosterSuccess("Successfully updated communicationSetup");
        this.router.navigate([RoutesLaout.AvCommunication,RoutesAvCommunication.CommunicationSetup]);
      });
    }

    if (this.communicationSetup.status == 0) {
      this.alertService.confirm(`InActive setup can't be reverted to active. Are your sure to make the setup inactive?`,
        () => { update(); },
        () => {}
      );
      return;
    }

    this.communicationService.getExistingCommunicationSetup({data:[this.communicationSetup]}).subscribe(res=>{
      res = res.filter(x=>x.id !== this.communicationSetup.id)
      if(res.length) {
        if(this.alertService.loadingFlag) this.alertService.fnLoading(false);        

        this.alertService.confirm(`AvSetup already exist in Salespoint ${this.communicationSetup.salesPoint.name}. Do you want to stop the communicationSetups before the new communicationSetup starts?`, () => {
          update();
        },()=>{}); 

      }
      else update();      
    })
    
  }

  handleBack(){
    this.router.navigate([RoutesAvCommunication.Parent, RoutesAvCommunication.CommunicationSetup]);
  }



}
