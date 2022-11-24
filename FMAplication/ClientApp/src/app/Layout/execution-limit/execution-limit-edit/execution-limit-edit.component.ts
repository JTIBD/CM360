import { MinimumExecutionLimit } from './../../../Shared/Entity/minimum-execution-limit/minimum-execution-limit';
import { MinimumExecutionLimitService } from './../../../Shared/Services/minimum-execution-limit/minimum-execution-limit.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { RoutesExecutionLimit } from 'src/app/Shared/Routes/RoutesExecutionLimit';

@Component({
  selector: 'app-execution-limit-edit',
  templateUrl: './execution-limit-edit.component.html',
  styleUrls: ['./execution-limit-edit.component.css']
})
export class ExecutionLimitEditComponent implements OnInit {

  minimumExecutionLimit: MinimumExecutionLimit;
  canEditStartDate=false;
  targetVisitedOutlet: number;

  constructor(private route:ActivatedRoute,private executionLimitService: MinimumExecutionLimitService,
     private alertService:AlertService, private router: Router){
  }

  ngOnInit(){
    if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
      let id = this.route.snapshot.params.id;
      this.executionLimitService.getById(id).subscribe(res=>{
        this.minimumExecutionLimit = res.data;
      });
    }
  }

  submit(){  
    if(this.minimumExecutionLimit.targetVisitedOutlet < 0){
      this.alertService.tosterDanger("Target visited outlet can not be negative");
      return;
    }
    this.executionLimitService.getExistingMinimumExecutionLimit({data:[this.minimumExecutionLimit]}).subscribe(res=>{
      res = res.filter(x=>x.id !== this.minimumExecutionLimit.id)
      let update=()=>{
        this.executionLimitService.updateMinimumExecutionLimit(this.minimumExecutionLimit).subscribe(res=>{
          this.alertService.tosterSuccess("Successfully updated Target Outlet settings");
          this.router.navigate(["/configuration/execution-limit"]);
        });
      }
      if(res.length) {
        if(this.alertService.loadingFlag) this.alertService.fnLoading(false);        
        this.alertService.confirm(`Target Outlet setting already exist in Salespoint ${this.minimumExecutionLimit.salesPoint.name}. Do you want to stop the target outlet settings before the new target outline starts?`, () => {
          update();
        },()=>{}); 

      }
      else update();      
    })
    
  }

  handleBack(){
    this.router.navigate([RoutesExecutionLimit.Parent,RoutesExecutionLimit.MinimumExecutionLimit]);
  }

}
