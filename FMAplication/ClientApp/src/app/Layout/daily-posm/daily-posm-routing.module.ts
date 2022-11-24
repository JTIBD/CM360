import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DailyPosmAddComponent } from './daily-posm-add/daily-posm-add.component';
import { DailyPosmListComponent } from './daily-posm-list/daily-posm-list.component';

const routes: Routes = [
  {
    path: '',
    children: [
        { path: '', redirectTo: 'daily-posm-list' },
        { path: 'daily-posm-list', component: DailyPosmListComponent, data: { extraParameter: 'dailyposm' } },
        { path: 'daily-posm-add', component: DailyPosmAddComponent },
        { path: "daily-posm-add/:id", component:  DailyPosmAddComponent}
    ]
}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DailyPosmRoutingModule { }
