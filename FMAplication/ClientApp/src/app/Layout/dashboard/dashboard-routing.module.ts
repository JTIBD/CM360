import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoutesDashboard } from 'src/app/Shared/Routes/RoutesDashboard';
import { CommonComponent } from './common/common.component';


const routes: Routes = [
  {
    path: '',
    children: [
        { path: '', redirectTo: RoutesDashboard.Common },
        { path: RoutesDashboard.Common, component: CommonComponent, data: { extraParameter: 'dashboard' } },
       
    ]
}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
