import { TransactionNotification } from 'src/app/Shared/Entity/Notification/TransactionNotification';
import { TransactionWorkflowService } from './../../../Shared/Services/Transaction-Worklow/transaction-workflow.service';
import { NotificationService } from './../../../Shared/Services/Notification/notification.service';
import { Component, OnDestroy, OnInit } from "@angular/core";
import { CommonService } from "src/app/Shared/Services/Common/common.service";
import { InventoryManagementService } from "../inventory-management.service";
import { NgbModalOptions, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ModalSKUPickerComponent } from "../modal-skupicker/modal-skupicker.component";
import { ActivatedRoute, Router } from "@angular/router";
import {
  IPosmProductStock
} from "src/app/Shared/Entity/Inventory/StockProduct";
import {
  IStockAdjustmentTransaction,
  IStockStockAdjustmentItem,
} from "src/app/Shared/Entity/Inventory/StockAdjustmentTransaction";
import { AlertService } from "src/app/Shared/Modules/alert/alert.service";
import { WareHouse } from "src/app/Shared/Entity/Inventory";
import { Utility } from "src/app/Shared/utility";
import { EventService } from 'src/app/Shared/Services/EventService/event.service';
import { Subscription } from 'rxjs';
import { TransactionType } from 'src/app/Shared/Enums/TransactionType';

@Component({
  selector: "app-central-warehouse-stock-adjust",
  templateUrl: "./central-warehouse-stock-adjust.component.html",
  styleUrls: ["./central-warehouse-stock-adjust.component.css"],
})
export class CentralWarehouseStockAdjustComponent implements OnInit, OnDestroy {
  isNew: boolean;
  transactionData: IStockAdjustmentTransaction;
  selectedItems: IPosmProductStock[] = [];
  selectedWarehouseId: number;
  wareHouses: WareHouse[] = [];
  loading: boolean = false;
  transactionDate: string;

  transactionId;

  notificationUpdatedSubscription : Subscription;
  constructor(
    private inventoryManagementService: InventoryManagementService,
    private alert: AlertService,
    private router: Router,
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private common: CommonService,
    private tWorkflowService: TransactionWorkflowService, 
    private eventService:EventService

  ) {

    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit() {
    if (
      Object.keys(this.route.snapshot.params).length !== 0 &&
      this.route.snapshot.params.id !== "undefined"
    ) {
      this.transactionId = this.route.snapshot.params.id;
      this.getTransactionById(this.transactionId);
      this.isNew = false;
    } else {
      this.isNew = true;
      this.getWarehouseData();
      this.getTransactionData();
    }

   
    this.notificationUpdatedSubscription = this.eventService.subscribe(
      'NotificationUpdateEvent',
      (response) => {
        this.getTransactionById(this.transactionId);
      });
  
  }

  ngOnDestroy(): void {
    if (this.notificationUpdatedSubscription)
        this.notificationUpdatedSubscription.unsubscribe();
   }

  getWarehouseData() {
    this.inventoryManagementService
      .getWareHouses()
      .subscribe((data: WareHouse[]) => {
        this.wareHouses = data;
      });
  }
  onWarehouseChange(warehouseId: number) {
    this.selectedWarehouseId = warehouseId;
  }

  getTransactionById(transactionId: number) {
    this.loading = true;
    this.inventoryManagementService
      .getStockAdjustmentTransactionById(transactionId)
      .subscribe((data: IStockAdjustmentTransaction) => {
        this.transactionData = data;
        // this.transactionData.transactionDate = Utility.getDateToStringFormat(data.transactionDate);
        this.transactionData.transactionDate = new Date(data.transactionDate).toISOString()
        this.transactionDate = Utility.getDateToStringFormat(this.transactionData.transactionDate);
        this.transactionData.transactionNotification = data.transactionNotification;
        this.selectedWarehouseId = this.transactionData.warehouseId;
        this.selectedItems = [];
        this.transactionData.stockAdjustmentItems.forEach(
          (item: IStockStockAdjustmentItem) => {
            let selectedItem: IPosmProductStock = {
              ...item.posmProduct,
              quantity: item.systemQuantity,
              adjustedQuantity: item.adjustedQuantity,
              availableQuantity:0,
            };
            this.selectedItems.push(selectedItem);
          }
        );
        this.loading = false;
      }), error => {
        this.loading = false;
      };
  }

  getTransactionData() {
    this.inventoryManagementService
      .getStockAdjustmentTransaction()
      .subscribe((data: IStockAdjustmentTransaction) => {
        this.transactionData = data;
        this.transactionData.transactionDate = new Date(data.transactionDate).toISOString()
        this.transactionDate = Utility.getDateToStringFormat(this.transactionData.transactionDate);

      });
  }

  selectProduct() {
    if (!this.selectedWarehouseId) {
      this.alert.tosterDanger("To Select SKU Product, You must need to select a warehouse");
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
    modalRef.componentInstance.selectedWareHouse = this.selectedWarehouseId;
    modalRef.result.then(
      (result) => {
        if (result && result.products) {
          this.selectedItems = result.products;
          this.selectedWarehouseId = result.selectedWarehouseId;
        }
      },
      (reason) => {
        console.log(reason);
      }
    );
  }

  removeItem(code: string) {
    if (code != "") {
      this.selectedItems = this.selectedItems.filter((x) => x.code != code);
    }
  }

  saveTransaction() {
    this.transactionData.stockAdjustmentItems = [];
    this.selectedItems.forEach((item) => {
      let product: IStockStockAdjustmentItem = {} as IStockStockAdjustmentItem;
      product.adjustedQuantity = +item.adjustedQuantity;
      product.posmProductId = item.id;
      product.systemQuantity = item.quantity;
      this.transactionData.stockAdjustmentItems.push(product);
    });
    this.transactionData.warehouseId = this.selectedWarehouseId;


    if (this.transactionData.id > 0) {
      this.inventoryManagementService
        .UpdateStockAdjustmentTransaction(this.transactionData)
        .subscribe(
          (data) => {
            this.router.navigate(["/inventory/stock-adjustment-list"]);
          },
          (error) => {
            this.alert.fnLoading(false);
            this.displayError(error);

          },
        );
    } else {
      this.inventoryManagementService
        .saveStockAdjustmentTransaction(this.transactionData)
        .subscribe(
          (data) => {
            this.router.navigate(["/inventory/stock-adjustment-list"]);
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
    this.router.navigate(["/inventory/stock-adjustment-list"]);
  }

  accept(workflowNotification: TransactionNotification) {

    var model = {
      "transactionId": workflowNotification.transactionId,
      "transactionWorkFlowId": workflowNotification.transactionWorkFlowId, 
      "transactionType" : TransactionType.StockAdjustment
    }
    this.tWorkflowService.acceptWorkFlow(model).subscribe(data => {
      this.getTransactionById(this.transactionId);
      this.NotificationRefreshBroadCast();
    })
  }
  reject(workflowNotification: TransactionNotification) {
    var model = {
      "transactionId": workflowNotification.transactionId,
      "transactionWorkFlowId": workflowNotification.transactionWorkFlowId, 
      "transactionType" : TransactionType.StockAdjustment
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
