import { Component, OnInit } from '@angular/core';
import { DailyPosmService } from 'src/app/Shared/Services/DailyActivity/daily-posm.service';
import { Router } from '@angular/router';
import { DailyPosm } from 'src/app/Shared/Entity/Daily-posm/daily-posm';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { PosmInstallationStatus } from 'src/app/Shared/Enums/posm-installation-status.enum';


@Component({
  selector: 'app-daily-posm-list',
  templateUrl: './daily-posm-list.component.html',
  styleUrls: ['./daily-posm-list.component.css']
})


export class DailyPosmListComponent implements OnInit {
  heading = 'Create A New Survey';
  subheading = 'Create new survey with multiple questions';
  icon = 'pe-7s-drawer icon-gradient bg-happy-itmeo';
  public enumPOSMStatus  = PosmInstallationStatus;
  public tosterMsgDltSuccess: string = "Record has been deleted successfully.";
  public tosterMsgError: string = "Something went wrong!";

  constructor(
    private router: Router,
    private dailyPOSMService: DailyPosmService,
    private alertService: AlertService
  ) { }

  dailyPOSMList: DailyPosm[] = [];

  ngOnInit() {
    this.getAllSuevey();
  }

  createNewDailyPOSM() {
    this.router.navigate(['/daily-posm/daily-posm-add']);
  }

  getAllSuevey() {
    this.alertService.fnLoading(true);
    this.dailyPOSMService.getDailyPOSMtList().subscribe(
      (res: any) => {
        this.dailyPOSMList = res.data;
        this.dailyPOSMList.forEach(s => s.posmInstallationStatus = this.enumPOSMStatus[s.posmInstallationStatus]);
        this.dailyPOSMList.forEach(s => s.posmRemovalStatus = this.enumPOSMStatus[s.posmRemovalStatus]);
        this.dailyPOSMList.forEach(s => s.posmRepairStatus = this.enumPOSMStatus[s.posmRepairStatus]);
        console.log(this.enumPOSMStatus);
      },
      (error) => {
        console.log(error);
        this.alertService.fnLoading(false);
        this.alertService.tosterDanger(this.tosterMsgError);
      },
      () => this.alertService.fnLoading(false));
  }

  edit(id: number) {
    console.log("DailyPOSm Id:", id);
    this.router.navigate([`/daily-posm/daily-posm-add/${id}`]);
  }

  delete(id: number) {
    this.alertService.confirm("Are you sure you want to delete this item?",
      () => {
        this.alertService.fnLoading(true);
        this.dailyPOSMService.deleteDailyPOSM(id).subscribe(
          (succ: any) => {
            console.log(succ.data);
            this.alertService.tosterSuccess(this.tosterMsgDltSuccess);
            this.getAllSuevey();
          },
          (error) => {
            console.log(error);
            this.alertService.fnLoading(false);
            this.alertService.tosterDanger(this.tosterMsgError);
          },
          () => this.alertService.fnLoading(false));
      }, () => { });
  }

  public ptableSettings = {
    tableID: "DailyPOSM-table",
    tableClass: "table-responsive",
    tableName: 'Daily POSM List',
    tableRowIDInternalName: "Id",
    tableColDef: [
      
      { headerName: 'Installation Status', width: '10%', internalName: 'posmInstallationStatus', sort: true, type: "" },
      { headerName: 'Repair Status', width: '10%', internalName: 'posmRepairStatus', sort: true, type: "" },
      { headerName: 'Removal Status', width: '10%', internalName: 'posmRemovalStatus', sort: true, type: "" },
      { headerName: 'POSM Installation Incomplete Reason', width: '20%', internalName: 'posmInstallationIncompleteReason', sort: false, type: "" },
      { headerName: 'POSM Repair Incomplete Reason', width: '20%', internalName: 'posmRepairIncompleteReason', sort: false, type: "" },
      { headerName: 'POSM Removal Incomplete Reason', width: '20%', internalName: 'posmRemovalIncompleteReason', sort: false, type: "" },
     
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 10,
    enabledPagination: true,
    enabledEditDeleteBtn: true,
    enabledColumnFilter: true,
    enabledRecordCreateBtn: true,
  };

  public fnCustomTrigger(event) {
    console.log("custom  click: ", event);

    if (event.action == "new-record") {
      this.createNewDailyPOSM();
    }
    else if (event.action == "edit-item") {
      this.edit(event.record.id);
    }
    else if (event.action == "delete-item") {
      this.delete(event.record.id);
    }
  }
  
}
