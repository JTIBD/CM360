import { Component, OnInit } from '@angular/core';
import { CampaignService } from 'src/app/Shared/Services/Campaign/campaign.service';
import { Campaign } from 'src/app/Shared/Entity/campaigns/campaign';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from "../../../Shared/Enums/status";
import { AlertService } from "../../../Shared/Modules/alert/alert.service";
import { MapObject } from "../../../Shared/Enums/mapObject";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { NgbCalendar, NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-campaign-add',
    templateUrl: './campaign-add.component.html',
    styleUrls: ['./campaign-add.component.css']
})
export class CampaignAddComponent implements OnInit {

    campModel: Campaign = new Campaign();
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    selectedStartDate: NgbDateStruct;
    selectedEndDate: NgbDateStruct;

    constructor(private alertService: AlertService, 
        private route: ActivatedRoute, 
        private calendar: NgbCalendar, 
        private campaignService: CampaignService, 
        private router: Router) { }
    ngOnInit() {
        this.createForm();
        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let productId = this.route.snapshot.params.id;
            this.getCampaign(productId);
        }
    }

    createForm() {
    }
    
    public fnRouteCampList() {
        this.router.navigate(['/campaign/campaign-list']);
    }

    private getCampaign(productId) {
        this.campaignService.getCampaign(productId).subscribe(
            (result: any) => {
                console.log("campaign data", result.data);
                this.editForm(result.data);
            },
            (err: any) => console.log(err)
        );
    };

    private editForm(campaign: Campaign) {
        this.campModel.id = campaign.id;
        this.campModel.campaignName = campaign.campaignName;
        this.campModel.campaignDetails = campaign.campaignDetails;
        this.campModel.startDate = campaign.startDate;
        this.campModel.endDate = campaign.endDate;
        this.campModel.status = campaign.status;

        this.selectedStartDate = this.ngDateToNgbDate(this.campModel.startDate);
        this.selectedEndDate = this.ngDateToNgbDate(this.campModel.endDate);
        
        console.log("campaign data edit after", this.campModel);
    }

    public fnSaveCampaign() {
        let hasError = false;
        let errorMsg = "";
        if (!this.selectedStartDate || !this.validateDate(this.selectedStartDate) || !this.selectedEndDate || !this.validateDate(this.selectedEndDate)) {
            hasError = true;
            errorMsg = "Please select valid Start and End date";
        }
      
        if (hasError) {
            this.alertService.tosterDanger(errorMsg);
            return;
        }

        this.campModel.startDate = this.ngbDateToNgDate(this.selectedStartDate);
        this.campModel.endDate = this.ngbDateToNgDate(this.selectedEndDate);

        this.campModel.id == 0 ? this.insertCampaign(this.campModel) : this.updateCampaign(this.campModel);
    }

    private insertCampaign(model: Campaign) {
        model.campaignName = model.campaignName.trim();
        this.campaignService.postCampaign(model).subscribe(res => {
            console.log("Campaign res: ", res);
            this.router.navigate(['/campaign/campaign-list']).then(() => {
                this.alertService.tosterSuccess("Campaign has been created successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private updateCampaign(model: Campaign) {
        model.campaignName = model.campaignName.trim();
        this.campaignService.putCampaign(model).subscribe(res => {
            console.log("Campaign upd res: ", res);
            this.router.navigate(['/campaign/campaign-list']).then(() => {
                this.alertService.tosterSuccess("Campaign has been edited successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private displayError(errorDetails: any) {
        // this.alertService.fnLoading(false);
        console.log("error", errorDetails);
        let errList = errorDetails.error.errors;
        if (errList.length) {
            console.log("error", errList, errList[0].errorList[0]);
            this.alertService.tosterDanger(errList[0].errorList[0]);
        } else {
            this.alertService.tosterDanger(errorDetails.error.msg);
        }
    }

    //#region helper method
    validateDate(date: NgbDateStruct | null): NgbDate | null {
      return date && this.calendar.isValid(NgbDate.from(date))
        ? NgbDate.from(date)
        : null;
    }
  
    dateToString(date: NgbDateStruct): string {
      if (this.validateDate(date)) {
        return `${date.year}-${date.month}-${date.day}`;
      }
      return "";
    }
  
    ngbDateToNgDate(date: NgbDateStruct): Date | null {
      if (this.validateDate(date)) {
        return new Date(date.year, date.month-1, date.day);
      }
      return null;
    }
  
    ngDateToNgbDate(date: Date): NgbDateStruct | null {
      if (date != null) {
          let newDate = new Date(date);
        let ngbDateStruct = { day: newDate.getDate(), month: newDate.getMonth()+1, year: newDate.getFullYear() } as NgbDateStruct;
        return ngbDateStruct;
      }
      return null;
    }
  
    getWithoutRef(value) {
      return JSON.parse(JSON.stringify(value));
    }
    //#endregion
}
