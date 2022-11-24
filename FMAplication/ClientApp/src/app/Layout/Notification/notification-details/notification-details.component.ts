import { NotificationService } from './../../../Shared/Services/Notification/notification.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { WorkflowLog } from 'src/app/Shared/Entity/WorkFlows/workflow-log';
import { Router } from '@angular/router';
import { WorkflowLogService } from 'src/app/Shared/Services/Workflow/workflow-log.service';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalNotificationComponent } from '../../LayoutComponent/Components/header/modal-notification/modal-notification.component';
import { WorkflowLogHistoryService } from 'src/app/Shared/Services/Workflow/workflow-log-history.service';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { Enums } from 'src/app/Shared/Enums/enums';
import { WorkflowLogHistory } from 'src/app/Shared/Entity/WorkFlows/workflow-log-history';
import { finalize } from 'rxjs/operators';
import { forkJoin, Subscription } from 'rxjs';
import { WorkflowStatusEnum, WorkflowStatusEnumLabel } from 'src/app/Shared/Enums/workflowStatusEnum';
import { TransactionNotification } from 'src/app/Shared/Entity/Notification/TransactionNotification';
import { TransactionWorkflowService } from 'src/app/Shared/Services/Transaction-Worklow/transaction-workflow.service';
import { EventService } from 'src/app/Shared/Services/EventService/event.service';
import { TransactionType } from 'src/app/Shared/Enums/TransactionType';

@Component({
  selector: 'app-notification-details',
  templateUrl: './notification-details.component.html',
  styleUrls: ['./notification-details.component.css']
})
export class NotificationDetailsComponent implements OnInit,  OnDestroy {

  tosterMsgDltSuccess: string = "Record Has been deleted successfully.";
  tosterMsgError: string = "Something went wrong!";

  workflowLogList: WorkflowLog[] = [];
  workflowLogHistoryList: WorkflowLogHistory[] = [];
  // historyCount : number = 0;
  closeResult: string;
  pendingWorkflowCount : number = 0;
  workFlowStatusEnumLabel : MapObject[] =  WorkflowStatusEnumLabel.workflowStatusEnumLabel;
  workFlowStatusEnum = WorkflowStatusEnum;
  tabOpen: number = 1;
  notifications:TransactionNotification[] = [];
  
  
  
  notificationUpdatedSubscription : Subscription;
 constructor(
   private router: Router,
   private workflowLogService: WorkflowLogService,
   private workflowLogHistoryService: WorkflowLogHistoryService,
   private alertService: AlertService,
   private modalService: NgbModal,
   private notification:NotificationService, 
   private tWorkflowService:TransactionWorkflowService,
   private eventService:EventService,
   private notificationService:NotificationService,
 
 ) { }

  ngOnDestroy(): void {
   if (this.notificationUpdatedSubscription)
       this.notificationUpdatedSubscription.unsubscribe();
  }

 ngOnInit() {

  this.getAllNotifications();

  this.notificationUpdatedSubscription = this.eventService.subscribe(
    'NotificationUpdateEvent',
    (response) => {
     this.getAllNotifications();
    });

 }

 getAllNotifications() { 
   this.notification.getAllNotifications().subscribe(data => {
     this.notifications = data;
     this.markAsRead();
   })
 }


 NotificationRefreshBroadCast(){
  this.eventService.broadcast({name : "notificationRefresh", content:''})
}
 
 fnRouteNotification(){
   this.router.navigate(['/notification/notification-details']);
 }

  accept(notification: TransactionNotification) {
    var model = {
      "transactionId": notification.transactionId,
      "transactionWorkFlowId": notification.transactionWorkFlowId, 
      "transactionType" : notification.transactionType
    }
    this.tWorkflowService.acceptWorkFlow(model).subscribe(data => {
      this.getAllNotifications();
      this.NotificationRefreshBroadCast();
    })
    
  }
  reject(notification: TransactionNotification) {
    var model = {
      "transactionId": notification.transactionId,
      "transactionWorkFlowId": notification.transactionWorkFlowId, 
      "transactionType" : notification.transactionType
    }
    this.tWorkflowService.rejectWorkFlow(model).subscribe(data => {
      this.getAllNotifications();
      this.NotificationRefreshBroadCast();
    })
   
  }

  navigateToTransaction(notification: TransactionNotification) {
    //CW salespoint adjustment = 1, SP salespoint adjustment = 4
    if (notification && notification.transaction &&  notification.transaction.transactionType === 1)
    this.router.navigate(['/inventory/stock-adjustment/' + notification.transaction.id]);
  else if (notification && notification.transaction &&  notification.transaction.transactionType === 4)
    this.router.navigate(['/inventory/salespoint-stock-adjustment/' + notification.transaction.id]);
  else if (notification && notification.transaction &&  notification.transactionType === TransactionType.SP_Transfer )
    this.router.navigate(['/inventory/salespoint-transfer-details/' + notification.transactionId]);
  }
  markAsRead() {
    this.notificationService.markAsRead().subscribe(data => { 
      this.NotificationRefreshBroadCast()
    })
  }
  

 
}
