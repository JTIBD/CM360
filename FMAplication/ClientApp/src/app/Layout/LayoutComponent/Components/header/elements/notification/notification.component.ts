import { Transaction } from './../../../../../../Shared/Entity/Inventory/Transaction';
import { TransactionWorkflowService } from './../../../../../../Shared/Services/Transaction-Worklow/transaction-workflow.service';
import { NotificationService } from './../../../../../../Shared/Services/Notification/Notification.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { WorkflowLog } from 'src/app/Shared/Entity/WorkFlows/workflow-log';
import { Router } from '@angular/router';
import { WorkflowLogService } from 'src/app/Shared/Services/Workflow/workflow-log.service';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { trigger, state, animate, transition, keyframes, style } from '@angular/animations';
import { WorkflowLogHistory } from 'src/app/Shared/Entity/WorkFlows/workflow-log-history';
import { NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalNotificationComponent } from '../../modal-notification/modal-notification.component';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { Enums } from 'src/app/Shared/Enums/enums';
import { WorkflowLogHistoryService } from 'src/app/Shared/Services/Workflow/workflow-log-history.service';
import { WorkflowStatusEnumLabel, WorkflowStatusEnum } from 'src/app/Shared/Enums/workflowStatusEnum';
import { HubConnectionBuilder, LogLevel, HubConnection } from "@microsoft/signalr";
import { TransactionNotification, Notification } from 'src/app/Shared/Entity/Notification/TransactionNotification';
import { EventService } from 'src/app/Shared/Services/EventService/event.service';
import { Subscription } from 'rxjs/internal/Subscription';
import { TransactionStatus } from 'src/app/Shared/Enums/TransactionStatus';
import { TransactionType } from 'src/app/Shared/Enums/TransactionType';



@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css'],
  animations: [
    trigger('slideInOutAnimation', [
      transition(':enter', [
        animate(".6s", keyframes([
          style({
            opacity: '0',
            transform: 'translateX(-30px)'
          }),
          style({
            opacity: '0.5',
            transform: 'translateX(15px)'
          }),
          style({
            opacity: '1',
            transform: ' translateX(0)'
          })
        ]))
      ]),
      // transition(':leave', [
      //     animate('.6s ease-in-out', style({
      //         right: '-400%',
      //         backgroundColor: 'rgba(0, 0, 0, 0)'
      //     }))
      // ])
    ])
  ]
})
export class NotificationComponent implements OnInit, OnDestroy {


  tosterMsgDltSuccess: string = "Record Has been deleted successfully.";
  tosterMsgError: string = "Something went wrong!";

  workflowLogList: WorkflowLog[] = [];
  workflowLogHistoryList: WorkflowLogHistory[] = [];
  pendingWorkflowCount: number = 0;
  closeResult: string;
  workFlowStatusEnumLabel: MapObject[] = WorkflowStatusEnumLabel.workflowStatusEnumLabel;
  workFlowStatusEnum = WorkflowStatusEnum;

  notificationList:TransactionNotification[] = []; 
  notificationCount = 0;
 


  hubConnection: HubConnection;
  userId:string; 


  notificationRefreshSubscription : Subscription;

  constructor(
    private router: Router,
    private workflowLogService: WorkflowLogService,
    private workflowLogHistoryService: WorkflowLogHistoryService,
    private alertService: AlertService,
    private modalService: NgbModal,
    private notificationService:NotificationService,
    private tWorkflowService:TransactionWorkflowService,
    private eventService:EventService

  ) {

  }

  ngOnInit() {
    this.getNotifications();

    setTimeout(() => {
      
      this.startConnection();
     
      this.NewConnection();
      this.MessageReceived();
      this.NewNotificationReceived();
    }, 2000);

    this.notificationRefreshSubscription = this.eventService.subscribe(
      'notificationRefresh',
      (response) => {
       this.getNotifications();
      });
  }

  getNotifications(){
    this.notificationService.getTransactionNotifications().subscribe((data:Notification) =>   {
      this.notificationList = data.transactionNotifications;
      this.notificationCount = data.count;


    }) 
  }

  getStatusText(status) {
    const list = this.workFlowStatusEnumLabel.filter(k => k.id == status);
    return list.length > 0 ? list[0].label : 'NULL';
  }

  NotificationUpdateBroadCast(){
    this.eventService.broadcast({name : "NotificationUpdateEvent", content:''})
  }

  getWorkflowLogForCurrentUser() {
    this.alertService.fnLoading(true);
    this.workflowLogService.getWorkflowLogForCurrentUser(this.workFlowStatusEnum.Pending, 1, 10).subscribe(
      (res: any) => {
        this.workflowLogList = res.data;
        this.pendingWorkflowCount = res.data.length > 0 ? res.data[0].pendingWorkflowCount : 0;
        console.log(this.workflowLogList);
      },
      (error) => {
        console.log(error);
        this.alertService.fnLoading(false);
        //this.alertService.tosterDanger(this.tosterMsgError);
      },
      () => this.alertService.fnLoading(false));
  }


  accept(notification:TransactionNotification){
    var model = {
        "transactionId" : notification.transactionId, 
        "transactionWorkFlowId" : notification.transactionWorkFlowId, 
        "transactionType" : notification.transactionType
    }
    this.tWorkflowService.acceptWorkFlow(model).subscribe(data => {
      this.getNotifications();
      this.NotificationUpdateBroadCast();
    });
    
  }
  reject(notification:TransactionNotification){
    var model = {
      "transactionId" : notification.transactionId, 
      "transactionWorkFlowId" : notification.transactionWorkFlowId, 
      "transactionType" : notification.transactionType
   }
   this.tWorkflowService.rejectWorkFlow(model).subscribe(data => {
    this.getNotifications();
    this.NotificationUpdateBroadCast();
   });
   
  }
  

  getWorkflowLogHistoryForCurrentUser() {
    this.alertService.fnLoading(true);
    this.workflowLogHistoryService.getWorkflowLogHistoryForCurrentUser(1, 10).subscribe(
      (res: any) => {
        this.workflowLogHistoryList = res.data;
        console.log(this.workflowLogHistoryList);
      },
      (error) => {
        console.log(error);
        this.alertService.fnLoading(false);
        this.alertService.tosterDanger(this.tosterMsgError);
      },
      () => this.alertService.fnLoading(false));
  }

  fnRouteNotification() {
    this.router.navigate(['/notification/notification-details']);
  }



  openWorkflowLogModal(workflowLog, status) {
    let ngbModalOptions: NgbModalOptions = {
      backdrop: 'static',
      keyboard: false
    };
    const modalRef = this.modalService.open(ModalNotificationComponent, ngbModalOptions);
    modalRef.componentInstance.workflowLogModel = workflowLog;
    modalRef.componentInstance.workflowStatus = status;

    modalRef.result.then((result) => {
      console.log(result);
      this.closeResult = `Closed with: ${result}`;
      
      // this.getMenus();
      // this.getWorkflowLogForCurrentUser();
      // this.getWorkflowLogHistoryForCurrentUser();

    },
      (reason) => {
        console.log(reason);
      });
  }

  ngOnDestroy(): void {
    if (this.hubConnection)
      this.hubConnection.stop();

    if (this.notificationRefreshSubscription)
        this.notificationRefreshSubscription.unsubscribe();
  }

  notificationDialogeClose(event) {
    if (event) {
      console.log('is open');
    } else {
      console.log('is closed');
      if (this.notificationCount > 0) 
          this.markAsRead();
    }
  }


  markAsRead() {
    this.notificationService.markAsRead().subscribe(data => { 
      this.getNotifications();
    })
  }
  
  public startConnection = () => {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('./NotificationHub')
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started'); 
        this.onConnection();

      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }
  

  private onConnection = () => {
    const userInfo = JSON.parse(localStorage.getItem('userinfo'));
    let userId = userInfo.id ;
    if (userId != "") 
      this.hubConnection.invoke("Connect", `${userId}`).catch(err => console.error(err));
  }

  public NewConnection = () => {
    this.hubConnection.on('MyConnectionId', (data) => {
      console.log(data);
      //  this.onlineUserCount = data;
    });
  }

  public MessageReceived = () => {
    this.hubConnection.on('OnMessage', (data) => {
      console.log(data);
      //  this.onlineUserCount = data;
    });
  }
  public NewNotificationReceived = () => {
    this.hubConnection.on('OnSendNotificationToUser', (data) => {
     this.getNotifications();
    });
  }



  navigateToTransaction(notification:TransactionNotification){
    //CW salespoint adjustment = 1, SP salespoint adjustment = 4
    if (notification && notification.transaction &&  notification.transaction.transactionType === 1)
      this.router.navigate(['/inventory/stock-adjustment/' + notification.transaction.id]);
    else if (notification && notification.transaction &&  notification.transaction.transactionType === 4)
      this.router.navigate(['/inventory/salespoint-stock-adjustment/' + notification.transaction.id]);
    else if (notification && notification.transaction &&  notification.transactionType === TransactionType.SP_Transfer )
      this.router.navigate(['/inventory/salespoint-transfer-details/' + notification.transactionId]);
  }

}


