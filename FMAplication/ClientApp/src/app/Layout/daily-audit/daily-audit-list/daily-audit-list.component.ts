import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { DailyAuditService } from 'src/app/Shared/Services/DailyActivity/daily-audit.service';
import { PosmInstallationStatus } from 'src/app/Shared/Enums/posm-installation-status.enum';
import { DailyAudit } from 'src/app/Shared/Entity/Daily-Audit/daily-audit';

@Component({
  selector: 'app-daily-audit-list',
  templateUrl: './daily-audit-list.component.html',
  styleUrls: ['./daily-audit-list.component.css']
})
export class DailyAuditListComponent implements OnInit {

  heading = 'Create A New Survey';
  subheading = 'Create new survey with multiple questions';
  icon = 'pe-7s-drawer icon-gradient bg-happy-itmeo';
  public enumAuditStatus  = PosmInstallationStatus;
  public tosterMsgDltSuccess: string = "Record has been deleted successfully.";
  public tosterMsgError: string = "Something went wrong!";

  constructor(
    private router: Router,
    private dailyAuditService: DailyAuditService,
    private alertService: AlertService
  ) { }

  dailyAuditList: DailyAudit[] = [];

  ngOnInit() {
    this.getAllDailyAudit();
  }

  createNewDailyAudit() {
    this.router.navigate(['/daily-audit/daily-audit-add']);
  }

  getAllDailyAudit() {
    this.alertService.fnLoading(true);
    this.dailyAuditService.getDailyAuditList().subscribe(
      (res: any) => {
        this.dailyAuditList = res.data;
        console.log(this.dailyAuditList);
        this.dailyAuditList.forEach(s => s.distributionCheckStatus = this.enumAuditStatus[s.distributionCheckStatus]);
        this.dailyAuditList.forEach(s => s.facingCountStatus = this.enumAuditStatus[s.facingCountStatus]);
        this.dailyAuditList.forEach(s => s.planogramCheckStatus = this.enumAuditStatus[s.planogramCheckStatus]);
        this.dailyAuditList.forEach(s => s.priceAuditCheckStatus = this.enumAuditStatus[s.priceAuditCheckStatus]);
        console.log(this.dailyAuditList);
      },
      (error) => {
        console.log(error);
        this.alertService.fnLoading(false);
        this.alertService.tosterDanger(this.tosterMsgError);
      },
      () => this.alertService.fnLoading(false));
  }

  edit(id: number) {
    debugger;
    console.log("DailyAudit Id:", id);
    this.router.navigate([`/daily-audit/daily-audit-add/${id}`]);
  }

  delete(id: number) {
    this.alertService.confirm("Are you sure you want to delete this item?",
      () => {
        this.alertService.fnLoading(true);
        this.dailyAuditService.deleteDailyAudit(id).subscribe(
          (succ: any) => {
            console.log(succ.data);
            this.alertService.tosterSuccess(this.tosterMsgDltSuccess);
            this.getAllDailyAudit();
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
    tableID: "DailyAudit-table",
    tableClass: "table-responsive",
    tableName: 'Daily Audit List',
    tableRowIDInternalName: "Id",
    tableColDef: [
      
      { headerName: 'Distribution Check Status', width: '10%', internalName: 'distributionCheckStatus', sort: true, type: "" },
      { headerName: 'Faciing Count Status', width: '10%', internalName: 'facingCountStatus', sort: true, type: "" },
      { headerName: 'Planogram Check Status', width: '10%', internalName: 'planogramCheckStatus', sort: true, type: "" },
      { headerName: 'Price Audit Check Status', width: '10%', internalName: 'priceAuditCheckStatus', sort: true, type: "" },
      { headerName: 'Distribution Check Incomplete Reason', width: '15%', internalName: 'distributionCheckIncompleteReason', sort: false, type: "" },
      { headerName: 'Facing Count Check Incomplete Reason', width: '15%', internalName: 'facingCountCheckIncompleteReason', sort: false, type: "" },
      { headerName: 'Planogram Check Incomplete Reason', width: '15%', internalName: 'planogramCheckIncompleteReason', sort: false, type: "" },
      { headerName: 'Price Audit Check Incomplete Reason', width: '15%', internalName: 'priceAuditCheckIncompleteReason', sort: false, type: "" },
     
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
      this.createNewDailyAudit();
    }
    else if (event.action == "edit-item") {
      this.edit(event.record.id);
    }
    else if (event.action == "delete-item") {
      this.delete(event.record.id);
    }
  }
  

}
