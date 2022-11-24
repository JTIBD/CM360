import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DailyAuditAddComponent } from './daily-audit-add/daily-audit-add.component';
import { DailyAuditListComponent } from './daily-audit-list/daily-audit-list.component';


const routes: Routes = [

  {
    path: '',
    children: [
        { path: '', redirectTo: 'daily-audit-list' },
        { path: 'daily-audit-list', component: DailyAuditListComponent, data: { extraParameter: 'dailyaudit' } },
        { path: 'daily-audit-add', component: DailyAuditAddComponent },
        { path: "daily-audit-add/:id", component: DailyAuditAddComponent }
    ]
}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DailyAuditRoutingModule { }
