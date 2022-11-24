import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CentralWarehouseStockAddComponent } from './central-warehouse-stock-add/central-warehouse-stock-add.component';
import { InventoryManagementRoutingModule } from './inventory-management-routing-module';
import { InventoryManagementService } from './inventory-management.service';
import { CommonService } from '../../Shared/Services/Common/common.service';
import { CentralWarehouseStockAdjustComponent } from './central-warehouse-stock-adjust/central-warehouse-stock-adjust.component';
import { ModalSKUPickerComponent } from './modal-skupicker/modal-skupicker.component';
import { NgbModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { CentralWarehouseStockAdjustmentListComponent } from './central-warehouse-stock-adjustment-list/central-warehouse-stock-adjustment-list.component';
import { SharedMasterModule } from '../../Shared/Modules/shared-master/shared-master.module';
import { FormsModule } from '@angular/forms';
import { DownloadStockModalComponent } from './download-stock-modal/download-stock-modal.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { StockDistributionComponent } from './stock-distribution/stock-distribution.component';
import { DownloadStockDistributionModalComponent } from './download-stock-distribution-modal/download-stock-distribution-modal.component';
import { PaginatorModule } from 'primeng/paginator';
import { RecieveStockComponent } from './recieve-stock/recieve-stock.component';
import { RecievedTransactionsComponent } from './recieved-transactions/recieved-transactions.component';
import { SalespointStockAdjustmentListComponent } from './salespoint-stock-adjustment-list/salespoint-stock-adjustment-list.component';
import { SalespointStockAdjustmentComponent } from './salespoint-stock-adjustment/salespoint-stock-adjustment.component';
import { WareHouseTransferComponent } from './ware-house-transfer/ware-house-transfer.component';
import { DownloadWarehouseTransferModalComponent } from './download-warehouse-transfer-modal/download-warehouse-transfer-modal.component';
import { ReceiveWarehouseTransferComponent } from './receive-warehouse-transfer/receive-warehouse-transfer.component';
import { WarehouseReceivedTransfersComponent } from './warehouse-received-transfers/warehouse-received-transfers.component';
import { SalespointTransferComponent } from './salespoint-transfer/salespoint-transfer.component';
import { ModalDownloadSpTransferFormatComponent } from './modal-download-sp-transfer-format/modal-download-sp-transfer-format.component';
import { SalespointReceivedTransfersComponent } from './salespoint-received-transfers/salespoint-received-transfers.component';
import { ReceiveSalespointTransferComponent } from './receive-salespoint-transfer/receive-salespoint-transfer.component';
import { SalespointTransferDetailsComponent } from './salespoint-transfer-details/salespoint-transfer-details.component';




@NgModule({
  declarations: [
    CentralWarehouseStockAddComponent, 
    CentralWarehouseStockAdjustComponent, 
    ModalSKUPickerComponent, 
    CentralWarehouseStockAdjustmentListComponent,
    DownloadStockModalComponent, 
    StockDistributionComponent, DownloadStockDistributionModalComponent, RecieveStockComponent, RecievedTransactionsComponent, SalespointStockAdjustmentListComponent, SalespointStockAdjustmentComponent, WareHouseTransferComponent, DownloadWarehouseTransferModalComponent, ReceiveWarehouseTransferComponent, WarehouseReceivedTransfersComponent, SalespointTransferComponent, ModalDownloadSpTransferFormatComponent, SalespointReceivedTransfersComponent, ReceiveSalespointTransferComponent, SalespointTransferDetailsComponent
  ],
  imports: [
      CommonModule,
      InventoryManagementRoutingModule, 
      NgbModule, 
      AngularFontAwesomeModule, 
      SharedMasterModule,      
      FormsModule,
      NgSelectModule,
      PaginatorModule,
    ],
    providers: [
        InventoryManagementService,
        CommonService,
		NgbModal
    ], 
  entryComponents: [
      ModalSKUPickerComponent,DownloadStockModalComponent,DownloadStockDistributionModalComponent,DownloadWarehouseTransferModalComponent,ModalDownloadSpTransferFormatComponent,
  ]
})
export class InventoryManagementModule { }
