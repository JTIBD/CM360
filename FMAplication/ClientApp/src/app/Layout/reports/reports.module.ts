import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportsRoutingModule } from './reports-routing.module';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { PosmDistributionReportComponent } from './posm-distribution-report/posm-distribution-report.component';
import { SalespointMultiSelectorComponent } from './salespoint-multi-selector/salespoint-multi-selector.component';
import { CwStockUpdateReportComponent } from './cw-stock-update-report/cw-stock-update-report.component';
import { SpWisePosmLedgerReportComponent } from './sp-wise-posm-ledger-report/sp-wise-posm-ledger-report.component';
import { ExecutionReportComponent } from './execution-report/execution-report.component';


@NgModule({
  declarations: [PosmDistributionReportComponent, SalespointMultiSelectorComponent, CwStockUpdateReportComponent, SpWisePosmLedgerReportComponent, ExecutionReportComponent],
  imports: [
    CommonModule,
    ReportsRoutingModule,
    SharedMasterModule,
    ReactiveFormsModule,
    FormsModule,
    NgbModule,
    NgSelectModule
  ]
})
export class ReportsModule { }
