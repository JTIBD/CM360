import { SpWisePosmLedgerReportComponent } from './sp-wise-posm-ledger-report/sp-wise-posm-ledger-report.component';
import { CwStockUpdateReportComponent } from './cw-stock-update-report/cw-stock-update-report.component';
import { PosmDistributionReportComponent } from './posm-distribution-report/posm-distribution-report.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoutesReports } from 'src/app/Shared/Routes/RoutesReports';
import { ExecutionReportComponent } from './execution-report/execution-report.component';


const routes: Routes = [
{
  path: '',
  children:[
    {path: RoutesReports.PosmDistributionReport, component: PosmDistributionReportComponent},
    {path: RoutesReports.CwStockUpdateReport, component: CwStockUpdateReportComponent},
    {path: RoutesReports.SpWisePosmLedgerReport, component: SpWisePosmLedgerReportComponent},
    {path: RoutesReports.ExecutionReport, component: ExecutionReportComponent}
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportsRoutingModule { }
