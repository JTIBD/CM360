import { PosmAssignComponent } from './posm-assign/posm-assign.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CmTaskGenerationComponent } from './cm-task-generation/cm-task-generation.component';
import { CmTaskListComponent } from './cm-task-list/cm-task-list.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { CompleteReportComponent } from './complete-report/complete-report.component';
import { DCMAReportInDetailsListComponent } from './dcma-report-details-list/dcma-report-details-list.component';
import { DCMAReportSalesPointWiseListComponent } from './dcma-report-sales-point-wise-list/dcma-report-sales-point-wise-list.component';
import { DCMAReportTerritoryWiseListComponent } from './dcma-report-territory-wise-list/dcma-report-territory-wise-list.component';
import { DCMAReportRegionWiseListComponent } from './dcma-report-region-wise-list/dcma-report-region-wise-list.component';
import { DCMAReportAreaWiseListComponent } from './dcma-report-area-wise-list/dcma-report-area-wise-list.component';
import { WarehouseStockComponent } from './warehouse-stock/warehouse-stock.component';
import { SalespointStockComponent } from './salespoint-stock/salespoint-stock.component';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
const routes: Routes = [
  {
    path: '',
    children: [
      {path:'', redirectTo:'cm-task-generation'},
      { path: 'cm-task-generation', component: CmTaskGenerationComponent ,canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: '/task/cm-task-generation' } },
      { path: 'cm-task-list', component: CmTaskListComponent,canActivate: [PermissionGuard], data: { permissionType: 'view', permissionGroup: '/task/cm-task-list' } },
      { path: 'complete-report', component: CompleteReportComponent },
      { path: 'posm-assign', component: PosmAssignComponent, canActivate: [PermissionGuard], data: { permissionType: "view", permissionGroup: `${RoutesLaout.Task}/posm-assign`} },
      { path: 'dcma-report-details', component: DCMAReportInDetailsListComponent },
      { path: 'dcma-report-sales-point', component: DCMAReportSalesPointWiseListComponent },
      { path: 'dcma-report-territory', component: DCMAReportTerritoryWiseListComponent },
      { path: 'dcma-report-area', component: DCMAReportAreaWiseListComponent },
      { path: 'dcma-report-region', component: DCMAReportRegionWiseListComponent },
      { path: 'warehouse-stock', component: WarehouseStockComponent },
      { path: 'salespoint-stock', component: SalespointStockComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DailyActivityRoutingModule { }
