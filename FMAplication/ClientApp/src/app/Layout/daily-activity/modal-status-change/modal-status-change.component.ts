import { Component, OnInit } from '@angular/core';
import { UserInfo, DailyCMActivity, APIResponse, User } from 'src/app/Shared/Entity';
import { NgbActiveModal, NgbDateStruct, NgbDate, NgbCalendar, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CmTaskGenerationService } from 'src/app/Shared/Services/DailyActivity/cm-task-generation.service';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { BatchStatusChange } from 'src/app/Shared/Entity/Daily-Activity/batch-status-change';
import { UserService } from 'src/app/Shared/Services/Users';

@Component({
  selector: 'app-modal-status-change',
  templateUrl: './modal-status-change.component.html',
  styleUrls: ['./modal-status-change.component.css']
})
export class ModalStatusChangeComponent implements OnInit {

  cmUserList: User[] = [];
  model : BatchStatusChange = new BatchStatusChange();
  public selectedDate: NgbDateStruct;
  fromDate: NgbDate | null;
  enumStatusTypes: MapObject[] = StatusTypes.statusType.slice(0, 2);
  tosterMsgError: string = "Something went wrong";


  constructor(public activeModal: NgbActiveModal,
    private calendar: NgbCalendar,
    public formatter: NgbDateParserFormatter,
    private userService: UserService,
    private cmTaskGenerationService: CmTaskGenerationService,
    private alertService: AlertService) { 
      
    }

  ngOnInit() {
    this.fnGetCMUsers();
    this.fromDate = this.calendar.getToday();
    this.model.status = 1;
  }

  fnGetCMUsers() {
    this.userService.getUserList().subscribe((res: APIResponse) => {
      this.cmUserList = res.data || [];
    });
  }

  submit() {

    this.model.date = new Date(
      this.selectedDate.year,
      this.selectedDate.month - 1,
      this.selectedDate.day
    )
    
    this.cmTaskGenerationService.updateBatchStatus(this.model).subscribe(
      (res: any) => {
        console.log(res.data);
        this.activeModal.close("Status updated");
        this.alertService.titleTosterSuccess("CM Task status updated.");
      },
      (err) => {
        console.log(err);
        this.showError();
      }
    );
  }

  showError(msg: string = null) {
    this.activeModal.close(msg ? msg : this.tosterMsgError);
  }

}
