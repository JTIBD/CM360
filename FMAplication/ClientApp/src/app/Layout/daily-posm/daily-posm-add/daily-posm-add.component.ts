import { Component, OnInit } from '@angular/core';
import { DailyPosmService } from '../../../Shared/Services/DailyActivity/daily-posm.service';
import { Router, ActivatedRoute } from '@angular/router';
import { DailyPosm } from '../../../Shared/Entity/Daily-posm/daily-posm';
import { PosmInstallationStatus } from 'src/app/Shared/Enums/posm-installation-status.enum';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';

@Component({
    selector: 'app-daily-posm-add',
    templateUrl: './daily-posm-add.component.html',
    styleUrls: ['./daily-posm-add.component.css']
})
export class DailyPosmAddComponent implements OnInit {

    constructor(private dailyPOSMService: DailyPosmService,
         private router: Router,
        private alertService: AlertService,
        private route: ActivatedRoute) { }

    ngOnInit() {
        
        
        this.createForm();

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            // console.log("id", this.route.snapshot.params.id);
            let dailyPOSMId = this.route.snapshot.params.id;
            this.getDailyPOSM(dailyPOSMId);
          }
    }

    dailyPOSMModel: DailyPosm = new DailyPosm();
    choices : {};

    InstallationOptions : string[] ;
    InstallationOption : {};
   


    createForm()
    {
        this.InstallationOption = PosmInstallationStatus;
        this.InstallationOptions = Object.keys(this.InstallationOption).filter(k => isNaN(Number(k)));
       // this.keys = Object.keys(this.statusOptions).filter(k => !isNaN(Number(k)));
      //  debugger;
        console.log(this.InstallationOptions);
        this.fnGetDropdownValue();
   
    }

    fnGetDropdownValue()
    {
        this.dailyPOSMService.getDropdownValue().subscribe(
            (success) =>{
                console.log(success)
                this.choices = success.data;
            },
            (error) =>{
                console.log(error)
            }
        )
    }


    fnSaveDailyPOSM(model: DailyPosm) {
        console.log("DailyPOSM model: ", model);
        this.dailyPOSMService.postDailyPOSM(model).subscribe((res) => {
            console.log("DailyPOSM resp: ", res);
            this.router.navigate(['/daily-posm/daily-posm-list']).then( () =>{
              this.alertService.titleTosterSuccess("Record has been saved successfully.");
            });
        },
            (error) => {
               // debugger;
               this.displayError(error);
            }, () => this.alertService.fnLoading(false)
  
  
        );
    }

    fnRouteDailyPOSMList() {
        this.router.navigate(['/daily-posm/daily-posm-list']);
    }

    displayError(errorDetails: any) {
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

    getDailyPOSM(dailyPOSMId) {
        this.dailyPOSMService.getDailyPOSM(dailyPOSMId).subscribe(
          (result: any) => {
              debugger;
            console.log("DailyPOSM data", result.data);
            this.editForm(result.data);
          },
          (err: any) => console.log(err)
        );
      };
    
      editForm(dailyPOSMModelData: any) {
        this.dailyPOSMModel.id = dailyPOSMModelData.id;
        this.dailyPOSMModel.dailyCMActivityId = dailyPOSMModelData.dailyCMActivityId;
        this.dailyPOSMModel.isPOSMInstallation = dailyPOSMModelData.isPOSMInstallation;
        this.dailyPOSMModel.isPOSMRemoval = dailyPOSMModelData.isPOSMRemoval;
        this.dailyPOSMModel.isPOSMRepair = dailyPOSMModelData.isPOSMRepair;
        this.dailyPOSMModel.posmInstallationStatus = dailyPOSMModelData.posmInstallationStatus;
        this.dailyPOSMModel.posmRemovalStatus = dailyPOSMModelData.posmRemovalStatus;
        this.dailyPOSMModel.posmRepairStatus = dailyPOSMModelData.posmRepairStatus;
        this.dailyPOSMModel.posmInstallationIncompleteReason = dailyPOSMModelData.posmInstallationIncompleteReason;
        this.dailyPOSMModel.posmRemovalIncompleteReason = dailyPOSMModelData.posmRemovalIncompleteReason;
        this.dailyPOSMModel.posmRepairIncompleteReason = dailyPOSMModelData.posmRepairIncompleteReason;
    
      }

   

}



