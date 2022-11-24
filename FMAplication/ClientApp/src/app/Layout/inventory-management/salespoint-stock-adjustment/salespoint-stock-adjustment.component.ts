import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { TransactionWorkflowService } from './../../../Shared/Services/Transaction-Worklow/transaction-workflow.service';
import { TransactionNotification } from 'src/app/Shared/Entity/Notification/TransactionNotification';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { PosmProduct, SalesPoint } from 'src/app/Shared/Entity';
import { IPosmProductStock } from 'src/app/Shared/Entity/Inventory';
import { Transaction } from 'src/app/Shared/Entity/Inventory/Transaction';
import { SalesPointStockAdjustmentItem } from 'src/app/Shared/Entity/salesPointStockAdjustment';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { RoutesInventoryManagement } from 'src/app/Shared/Routes/RoutesInventoryManagement';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { UserService } from 'src/app/Shared/Services/Users';
import { Utility } from 'src/app/Shared/utility';
import { InventoryManagementService } from '../inventory-management.service';
import { ModalSKUPickerComponent } from '../modal-skupicker/modal-skupicker.component';
import { Subscription } from 'rxjs';
import { EventService } from 'src/app/Shared/Services/EventService/event.service';
import { TransactionType } from 'src/app/Shared/Enums/TransactionType';

@Component({
  selector: 'app-salespoint-stock-adjustment',
  templateUrl: './salespoint-stock-adjustment.component.html',
  styleUrls: ['./salespoint-stock-adjustment.component.css']
})
export class SalespointStockAdjustmentComponent implements OnInit, OnDestroy {

  isNew: boolean;
  transactionData: Transaction;
  //selectedItems: PosmProduct[] = [];
  selectedSalesPointId: number;
  salesPoints: SalesPoint[] = [];
  loading : boolean = false; 
  transactionDate : string;
  transactionId;

  notificationUpdatedSubscription : Subscription;
  constructor(
    private inventoryManagementService: InventoryManagementService,
    private alert: AlertService,
    private router: Router,
    private route: ActivatedRoute,
    private modalService: NgbModal, 
    private common : CommonService,
    private userService:UserService, 
    private tWorkflowService:TransactionWorkflowService, 
    private eventService:EventService

  ) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }
  ngOnDestroy(): void {
    if (this.notificationUpdatedSubscription)
        this.notificationUpdatedSubscription.unsubscribe();
  }

  ngOnInit() {
    if (
      Object.keys(this.route.snapshot.params).length !== 0 &&
      this.route.snapshot.params.id !== "undefined"
    ) {
      this.transactionId= this.route.snapshot.params.id;
      this.getTransactionById(this.transactionId);
      
      this.isNew = false;
    } else {
      this.isNew = true;
      this.getSalesPoints();
      this.transactionData = new Transaction();
    }

    this.notificationUpdatedSubscription = this.eventService.subscribe(
      'NotificationUpdateEvent',
      (response) => {
        this.getTransactionById(this.transactionId);
      });
  }

  getSalesPoints() {
    this.userService.getAllSalesPointByCurrentUser().subscribe((res) => {
     this.salesPoints =  [...res.data];
    });
  }

  getTransactionById(transactionId: number) {
    this.loading = true;
    this.inventoryManagementService
      .getSalesPointStockAdjustmentTransactionById(transactionId)
      .subscribe((data) => {
        this.transactionData = data;
        this.selectedSalesPointId = this.transactionData.salesPointId;        
        this.loading = false;
      }), error => {
        this.loading = false;
      };
  }

  selectProduct() {
    if (!this.selectedSalesPointId) {
      this.alert.tosterDanger("To Select SKU Product, You must need to select a salespoint");
      return;
    }
    let ngbModalOptions: NgbModalOptions = {
      backdrop: "static",
      keyboard: false,
      size: "lg",
    };
    const modalRef = this.modalService.open(
      ModalSKUPickerComponent,
      ngbModalOptions
    );
    modalRef.componentInstance.skuSource = "salespoint";
    modalRef.componentInstance.selectedSalesPointId = this.selectedSalesPointId;
    modalRef.result.then(
      (result) => {
        if (result && result.products) {
          const stocks:IPosmProductStock[] = result.products;
          this.transactionData.salesPointAdjustmentItems = stocks.map(x=> {
            const item = {
              posmProduct:x as PosmProduct,
              adjustedQuantity:x.adjustedQuantity,              
              posmProductId:x.id,
              systemQuantity:x.quantity,
            } as SalesPointStockAdjustmentItem;
            return item;
          });
          
        }
      },
      (reason) => {
        console.log(reason);
      }
    );
  }

  removeItem(posmProductId: number) {
    if (posmProductId != 0) {
      this.transactionData.salesPointAdjustmentItems = this.transactionData.salesPointAdjustmentItems.filter((x) => x.posmProductId != posmProductId);
    }
  }

  saveTransaction() {  
    this.transactionData.salesPointId = this.selectedSalesPointId;
    let invalidNumbers = this.transactionData.salesPointAdjustmentItems.filter(x=> isNaN(x.adjustedQuantity)).map(x=>x.adjustedQuantity);
    if(!!invalidNumbers.length) {
      this.alert.tosterDanger(`${invalidNumbers.join()} ${invalidNumbers.length==1?'is':'are'} not valid number`);
      return;
    }    
    this.transactionData.salesPointAdjustmentItems.forEach(x=>x.adjustedQuantity = Number(x.adjustedQuantity));

    if (this.transactionData.id > 0) {
      this.inventoryManagementService
        .UpdateSalesPointStockAdjustmentTransaction(this.transactionData)
        .subscribe(
          (data) => {
            this.router.navigate([RoutesLaout.Inventory,RoutesInventoryManagement.SalesPointStockAdjustmentList]);
          },
          (error) => {
            this.alert.fnLoading(false);
            this.displayError(error); 
           
          },
        );
    } else {
      this.inventoryManagementService
        .saveSalesPointStockAdjustmentTransaction(this.transactionData)
        .subscribe(
          (data) => {
            this.router.navigate([RoutesLaout.Inventory,RoutesInventoryManagement.SalesPointStockAdjustmentList]);
          },
          (error) => {
            this.alert.fnLoading(false);
            this.displayError(error); 
          },
        );
    }
  }

  private displayError(errorDetails: any) {
    console.log("error", errorDetails);
    let errList = errorDetails.error;
    if (errList.length) {
      console.log("error", errList, errList[0].errorList[0]);
      this.alert.tosterDanger(errList[0].errorList[0]);
    } else {
      if (errorDetails.error.Type === 0) {
        let message = errorDetails.error.Error.Message;
        if (message && message.length > 0) {
           setTimeout(() => {
            this.alert.alert(message);
          }, 1010);

        }
      }


    }
  }
  BackToPage() {
    this.router.navigate([RoutesLaout.Inventory,RoutesInventoryManagement.SalesPointStockAdjustmentList]);
  }



  accept(workflowNotification:TransactionNotification){
    
    var model = {
        "transactionId" : workflowNotification.transactionId, 
        "transactionWorkFlowId" : workflowNotification.transactionWorkFlowId, 
        "transactionType" : TransactionType.SalesPointStockAdjustment
    }
    this.tWorkflowService.acceptWorkFlow(model).subscribe(data => {
     this.getTransactionById(this.transactionId);
     this.NotificationRefreshBroadCast();
    })
  }
  reject(workflowNotification:TransactionNotification){
    var model = {
      "transactionId" : workflowNotification.transactionId, 
      "transactionWorkFlowId" : workflowNotification.transactionWorkFlowId, 
      "transactionType" : TransactionType.SalesPointStockAdjustment
      
   }
   this.tWorkflowService.rejectWorkFlow(model).subscribe(data => {
    this.getTransactionById(this.transactionId);
    this.NotificationRefreshBroadCast();
   })
  }

  NotificationRefreshBroadCast(){
    this.eventService.broadcast({name : "notificationRefresh", content:''})
  }

}
