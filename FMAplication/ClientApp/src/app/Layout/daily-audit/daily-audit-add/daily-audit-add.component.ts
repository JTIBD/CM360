import { Component, OnInit } from '@angular/core';
import { DailyAuditService } from 'src/app/Shared/Services/DailyActivity/daily-audit.service';
import { Router, ActivatedRoute } from '@angular/router';
import { DailyAudit } from 'src/app/Shared/Entity/Daily-Audit/daily-audit';
import { PosmInstallationStatus } from 'src/app/Shared/Enums/posm-installation-status.enum';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { FormGroup } from '@angular/forms';


@Component({
  selector: 'app-daily-audit-add',
  templateUrl: './daily-audit-add.component.html',
  styleUrls: ['./daily-audit-add.component.css']
})
export class DailyAuditAddComponent implements OnInit {

    public form: FormGroup;
    
  constructor(private dailyAuditService: DailyAuditService,
     private router: Router,
     private alertService: AlertService,
     private route: ActivatedRoute
     ) { }

    ngOnInit() {
        //this.fnGetDropdownValue();
        this.createForm();

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            // console.log("id", this.route.snapshot.params.id);
            let dailyAuditId = this.route.snapshot.params.id;
            this.getDailyAudit(dailyAuditId);
          }
    }

    dailyAuditModel: DailyAudit = new DailyAudit();
    InstallationOptions : string[] ;
    InstallationOption: {};
    choices: {};


    createForm()
    {
        this.InstallationOption = PosmInstallationStatus;
        this.InstallationOptions = Object.keys(this.InstallationOption).filter(k => isNaN(Number(k)));
      
        console.log(this.InstallationOptions);
        this.fnGetDropdownValue();
   
    }

     fnGetDropdownValue()
     {
         this.dailyAuditService.getDropdownValue().subscribe(
             (success) => {
                
                 this.choices = success.data;
                 console.log('choices', this.choices);
             },
             (error) =>{
                 console.log(error)
             }
         )
     }

   

    saveDailyAudit(model: DailyAudit) {
        console.log("DailyAudit model: ", model);
        this.dailyAuditService.postDailyAudit(model).subscribe((res) => {
            console.log("DailyAudit resp: ", res);
            this.router.navigate(['/daily-audit/daily-audit-list']).then( () =>{
              this.alertService.titleTosterSuccess("Record has been saved successfully.");
            });
        },
            (error) => {
               // debugger;
               this.displayError(error);
            }, () => this.alertService.fnLoading(false)
  
  
        );
    }

    dailyAuditList() {
        this.router.navigate(['/daily-audit/daily-audit-list']);
    }



    getDailyAudit(dailyAuditId) {
        this.dailyAuditService.getDailyAudit(dailyAuditId).subscribe(
          (result: any) => {
              
            console.log("DailyAudit data", result.data);
            this.editForm(result.data);
          },
          (err: any) => console.log(err)
        );
      };
    
      editForm(dailyAuditModelData: any) {
        this.dailyAuditModel.id = dailyAuditModelData.id;
        this.dailyAuditModel.dailyCMActivityId = dailyAuditModelData.dailyCMActivityId;
        this.dailyAuditModel.isDistributionCheck = dailyAuditModelData.isDistributionCheck;
        this.dailyAuditModel.isFacingCount = dailyAuditModelData.isFacingCount;
        this.dailyAuditModel.isPlanogramCheck = dailyAuditModelData.isPlanogramCheck;
        this.dailyAuditModel.isPriceAudit = dailyAuditModelData.isPriceAudit;
        this.dailyAuditModel.distributionCheckStatus = dailyAuditModelData.distributionCheckStatus;
        this.dailyAuditModel.facingCountStatus = dailyAuditModelData.facingCountStatus;
        this.dailyAuditModel.planogramCheckStatus = dailyAuditModelData.planogramCheckStatus;
        this.dailyAuditModel.priceAuditCheckStatus = dailyAuditModelData.priceAuditCheckStatus;
        this.dailyAuditModel.distributionCheckIncompleteReason = dailyAuditModelData.distributionCheckIncompleteReason;
        this.dailyAuditModel.facingCountCheckIncompleteReason = dailyAuditModelData.facingCountCheckIncompleteReason;
        this.dailyAuditModel.planogramCheckIncompleteReason = dailyAuditModelData.planogramCheckIncompleteReason;
        this.dailyAuditModel.priceAuditCheckIncompleteReason = dailyAuditModelData.priceAuditCheckIncompleteReason;
    
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


}
