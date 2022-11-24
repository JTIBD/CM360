import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { RoutesInventoryManagement } from 'src/app/Shared/Routes/RoutesInventoryManagement';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { CentralWarehouseStockAddComponent } from './central-warehouse-stock-add/central-warehouse-stock-add.component';
import { CentralWarehouseStockAdjustComponent } from './central-warehouse-stock-adjust/central-warehouse-stock-adjust.component';
import { CentralWarehouseStockAdjustmentListComponent } from './central-warehouse-stock-adjustment-list/central-warehouse-stock-adjustment-list.component';
import { ReceiveSalespointTransferComponent } from './receive-salespoint-transfer/receive-salespoint-transfer.component';
import { ReceiveWarehouseTransferComponent } from './receive-warehouse-transfer/receive-warehouse-transfer.component';
import { RecieveStockComponent } from './recieve-stock/recieve-stock.component';
import { RecievedTransactionsComponent } from './recieved-transactions/recieved-transactions.component';
import { SalespointReceivedTransfersComponent } from './salespoint-received-transfers/salespoint-received-transfers.component';
import { SalespointStockAdjustmentListComponent } from './salespoint-stock-adjustment-list/salespoint-stock-adjustment-list.component';
import { SalespointStockAdjustmentComponent } from './salespoint-stock-adjustment/salespoint-stock-adjustment.component';
import { SalespointTransferDetailsComponent } from './salespoint-transfer-details/salespoint-transfer-details.component';
import { SalespointTransferComponent } from './salespoint-transfer/salespoint-transfer.component';
import { StockDistributionComponent } from './stock-distribution/stock-distribution.component';
import { WareHouseTransferComponent } from './ware-house-transfer/ware-house-transfer.component';
import { WarehouseReceivedTransfersComponent } from './warehouse-received-transfers/warehouse-received-transfers.component';

const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'common' },
            { path: 'stock-add', component: CentralWarehouseStockAddComponent, canActivate: [PermissionGuard], data: {permissionType: "view", permissionGroup: `${RoutesLaout.Inventory}/stock-add`} },
            { path: 'stock-distribution', component: StockDistributionComponent, canActivate: [PermissionGuard], data: { permissionType: "view", permissionGroup: `${RoutesLaout.Inventory}/stock-distribution`} },
            { path: 'stock-adjustment', component: CentralWarehouseStockAdjustComponent, canActivate: [PermissionGuard], data: {permissionType: "create", permissionGroup: `${RoutesLaout.Inventory}/stock-adjustment-list`} },
            { path: 'stock-adjustment/:id', component: CentralWarehouseStockAdjustComponent, canActivate: [PermissionGuard], data: {permissionType: "update", permissionGroup: `${RoutesLaout.Inventory}/stock-adjustment-list`} },
            { path: 'stock-adjustment-list', component: CentralWarehouseStockAdjustmentListComponent, canActivate: [PermissionGuard], data: {permissionType: "view", permissionGroup: `${RoutesLaout.Inventory}/stock-adjustment-list`} },
            { path: 'stock-receives', component: RecievedTransactionsComponent, canActivate: [PermissionGuard], data: {permissionType: "view", permissionGroup: `${RoutesLaout.Inventory}/stock-receives`} },
            { path: 'stock-receives/new', component: RecieveStockComponent, canActivate: [PermissionGuard], data: {permissionType: "create", permissionGroup: `${RoutesLaout.Inventory}/stock-receives`} },
            { path: RoutesInventoryManagement.SalesPointStockAdjustmentList, component: SalespointStockAdjustmentListComponent, canActivate: [PermissionGuard], data: { permissionType: "view", permissionGroup: `${RoutesLaout.Inventory}/${RoutesInventoryManagement.SalesPointStockAdjustmentList}`} },
            { path: `${RoutesInventoryManagement.SalesPointStockAdjustment}`, component: SalespointStockAdjustmentComponent, canActivate: [PermissionGuard], data: { permissionType: "create", permissionGroup: `${RoutesLaout.Inventory}/${RoutesInventoryManagement.SalesPointStockAdjustmentList}`} },
            { path: `${RoutesInventoryManagement.SalesPointStockAdjustment}/:id`, component: SalespointStockAdjustmentComponent, canActivate: [PermissionGuard], data: { permissionType: "update", permissionGroup: `${RoutesLaout.Inventory}/${RoutesInventoryManagement.SalesPointStockAdjustmentList}`} },
            { path: `${RoutesInventoryManagement.WareHouseTransfer}`, component: WareHouseTransferComponent, canActivate: [PermissionGuard], data: { permissionType: "view", permissionGroup: `${RoutesLaout.Inventory}/${RoutesInventoryManagement.WareHouseTransfer}`} },
            { path: `${RoutesInventoryManagement.ReceiveWareHouseTransfer}`, component: ReceiveWarehouseTransferComponent, canActivate: [PermissionGuard], data: { permissionType: "create", permissionGroup: `${RoutesLaout.Inventory}/${RoutesInventoryManagement.WareHouseReceivedTransfer}`} },
            { path: `${RoutesInventoryManagement.WareHouseReceivedTransfer}`, component: WarehouseReceivedTransfersComponent, canActivate: [PermissionGuard], data: { permissionType: "view", permissionGroup: `${RoutesLaout.Inventory}/${RoutesInventoryManagement.WareHouseReceivedTransfer}`} },
            { path: `${RoutesInventoryManagement.SalesPointTransfer}`, component: SalespointTransferComponent, canActivate: [PermissionGuard], data: { permissionType: "view", permissionGroup: `${RoutesLaout.Inventory}/${RoutesInventoryManagement.SalesPointTransfer}`} },
            { path: `${RoutesInventoryManagement.SalesPointTransferDetails}/:id`, component: SalespointTransferDetailsComponent, canActivate: [PermissionGuard], data: { permissionType: "view", permissionGroup: `${RoutesLaout.Inventory}/${RoutesInventoryManagement.SalesPointTransfer}`} },
            { path: `${RoutesInventoryManagement.SalesPointReceivedTransfer}`, component: SalespointReceivedTransfersComponent, canActivate: [PermissionGuard], data: { permissionType: "view", permissionGroup: `${RoutesLaout.Inventory}/${RoutesInventoryManagement.SalesPointReceivedTransfer}`} },
            { path: `${RoutesInventoryManagement.ReceiveSalesPointTransfer}`, component: ReceiveSalespointTransferComponent, canActivate: [PermissionGuard], data: { permissionType: "create", permissionGroup: `${RoutesLaout.Inventory}/${RoutesInventoryManagement.SalesPointReceivedTransfer}`} },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class InventoryManagementRoutingModule { }