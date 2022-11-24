import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgSelectModule } from '@ng-select/ng-select';

import { DailyActivityRoutingModule } from './daily-activity-routing.module';
import { CmTaskGenerationComponent } from './cm-task-generation/cm-task-generation.component';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { PipeSharedModule } from 'src/app/Shared/Pipes/pipe-shared.module';
import { ModalCmTaskDetailsComponent } from './modal-cm-task-details/modal-cm-task-details.component';
import { CmTaskListComponent } from './cm-task-list/cm-task-list.component';
import { ModalStatusChangeComponent } from './modal-status-change/modal-status-change.component';
import { CompleteReportComponent } from './complete-report/complete-report.component';
import { DCMAReportInDetailsListComponent } from './dcma-report-details-list/dcma-report-details-list.component';
import { DCMAReportSalesPointWiseListComponent } from './dcma-report-sales-point-wise-list/dcma-report-sales-point-wise-list.component';
import { DCMAReportTerritoryWiseListComponent } from './dcma-report-territory-wise-list/dcma-report-territory-wise-list.component';
import { DCMAReportAreaWiseListComponent } from './dcma-report-area-wise-list/dcma-report-area-wise-list.component';
import { DCMAReportRegionWiseListComponent } from './dcma-report-region-wise-list/dcma-report-region-wise-list.component';
import { PaginatorModule } from 'primeng/paginator';
import { TableModule } from 'primeng/table';
import { PosmAssignComponent } from './posm-assign/posm-assign.component';
import { ModalImportPosmAssignComponent } from './posm-assign/modal-import-posm-assign/modal-import-posm-assign.component';
import { WarehouseStockComponent } from './warehouse-stock/warehouse-stock.component';
import { SalespointStockComponent } from './salespoint-stock/salespoint-stock.component';
@NgModule({
    declarations: [CmTaskGenerationComponent,
        ModalCmTaskDetailsComponent,
        CmTaskListComponent,
        ModalStatusChangeComponent,
        CompleteReportComponent,
        DCMAReportInDetailsListComponent,
        DCMAReportSalesPointWiseListComponent,
        DCMAReportTerritoryWiseListComponent,
        DCMAReportAreaWiseListComponent,
        DCMAReportRegionWiseListComponent,
        PosmAssignComponent,
        ModalImportPosmAssignComponent,
        WarehouseStockComponent,
        SalespointStockComponent,
    ],
    imports: [
        CommonModule,
        DailyActivityRoutingModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        AngularFontAwesomeModule,
        NgSelectModule,
        SharedMasterModule,
        PipeSharedModule,
        TableModule,
        PaginatorModule,
    ],
    entryComponents: [
        ModalCmTaskDetailsComponent,
        ModalStatusChangeComponent, 
        ModalImportPosmAssignComponent
    ]
})
export class DailyActivityModule { }
