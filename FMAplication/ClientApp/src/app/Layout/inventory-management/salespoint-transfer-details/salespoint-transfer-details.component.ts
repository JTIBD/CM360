import { TransactionWorkflowService } from './../../../Shared/Services/Transaction-Worklow/transaction-workflow.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SalesPointTransfer, SalesPointTransferDetailsTableData } from 'src/app/Shared/Entity';
import { TransactionNotification } from 'src/app/Shared/Entity/Notification/TransactionNotification';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { RoutesInventoryManagement } from 'src/app/Shared/Routes/RoutesInventoryManagement';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { SalesPointService } from 'src/app/Shared/Services/Sales';
import { Utility } from 'src/app/Shared/utility';
import { EventService } from 'src/app/Shared/Services/EventService/event.service';
import { TransactionType } from 'src/app/Shared/Enums/TransactionType';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-salespoint-transfer-details',
  templateUrl: './salespoint-transfer-details.component.html',
  styleUrls: ['./salespoint-transfer-details.component.css']
})
export class SalespointTransferDetailsComponent implements OnInit, OnDestroy {

  selectedTransaction:SalesPointTransfer=null;
  selectedTransferId = 0;

  detailsTablePazeSize = 5;

  stockDistributionRowDetailsTableData: SalesPointTransferDetailsTableData[]=[];


  public transactionDetailsPtableSettings: IPTableSetting = {
    tableID: "transactionDetailsPtable",
    tableClass: "table table-border ",
    tableName: "Transaction Details",
    tableRowIDInternalName: "id",
    tableColDef: [
      { 
        headerName: "POSM Name", 
        width: "20%", 
        internalName: "posmName",
        sort: true, 
        type: "",
       },
      { 
        headerName: "Quantity", 
        width: "20%", 
        internalName: "quantity", 
        sort: true, 
        type: "",
       },
   
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: this.detailsTablePazeSize,
    enabledPagination: true,
    //enabledAutoScrolled:true,
    //enabledCellClick: true,
    enabledColumnFilter: true,
    enabledDeleteBtn: false,
    enabledPrint:true,
    
  };

  permissionGroup: PermissionGroup = new PermissionGroup();
  notificationUpdatedSubscription : Subscription;

  constructor(
    private salespointService:SalesPointService,
    private route: ActivatedRoute,
    private router: Router,
    private alert:AlertService,
    private activityPermissionService: ActivityPermissionService,
    private tWorkflowService:TransactionWorkflowService, 
    private eventService:EventService

  ) {
    this.initPermissionGroup();
   }

  ngOnInit() {
    this.getIdFromRoute();
    this.getSalesPointTransfer();

    this.notificationUpdatedSubscription = this.eventService.subscribe(
      'NotificationUpdateEvent',
      (response) => {
        this.getSalesPointTransfer();
      });
  }
  ngOnDestroy(): void {
    if (this.notificationUpdatedSubscription)
        this.notificationUpdatedSubscription.unsubscribe();
  }

  private initPermissionGroup() {
    this.permissionGroup = this.activityPermissionService.getPermission(this.route.snapshot.data.permissionGroup);
  }

  getIdFromRoute(){
    if( Object.keys(this.route.snapshot.params).length === 0 ||
    this.route.snapshot.params.id == "undefined"){
      this.alert.titleTosterDanger("Transaction id not found.");      
    }
    else{
      this.selectedTransferId = this.route.snapshot.params.id;
    }
  }

  getSalesPointTransfer(){
    this.salespointService.getSalesPointTransferById(this.selectedTransferId).subscribe(res=>{
      this.selectedTransaction = res;
      this.mapSelectedTransactionToDetailsTableData();
    })
  }

  getTransactionDate(dateStr:string){
    return  Utility.getDateToStringFormat(dateStr);
  }

  mapSelectedTransactionToDetailsTableData(){
    let data:SalesPointTransferDetailsTableData[]=[];
    data = this.selectedTransaction.items.map(x=>{
      const item = new SalesPointTransferDetailsTableData();
      if(x.posmProduct){
        item.posmName = x.posmProduct.name;
      } 
      item.quantity = x.quantity;
      return item;
    });
    this.stockDistributionRowDetailsTableData = data;
    
  }
  handleBack(){
    this.router.navigate([RoutesInventoryManagement.Parent,RoutesInventoryManagement.SalesPointTransfer]);
  }

  accept(workflowNotification:TransactionNotification){
    
    var model = {
        "transactionId" : workflowNotification.transactionId, 
        "transactionWorkFlowId" : workflowNotification.transactionWorkFlowId, 
        "transactionType" : TransactionType.SP_Transfer
       
    }
    this.tWorkflowService.acceptWorkFlow(model).subscribe(data => {
      this.getSalesPointTransfer();
     this.NotificationRefreshBroadCast();
    })
  }
  reject(workflowNotification:TransactionNotification){
    var model = {
      "transactionId" : workflowNotification.transactionId, 
      "transactionWorkFlowId" : workflowNotification.transactionWorkFlowId, 
      "transactionType" : TransactionType.SP_Transfer
      
   }
   this.tWorkflowService.rejectWorkFlow(model).subscribe(data => {
    this.getSalesPointTransfer();
    this.NotificationRefreshBroadCast();
   })
  }

  NotificationRefreshBroadCast(){
    this.eventService.broadcast({name : "notificationRefresh", content:''})
  }

}
