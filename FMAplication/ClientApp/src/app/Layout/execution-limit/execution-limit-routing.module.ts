import { ExecutionLimitListComponent } from './execution-limit-list/execution-limit-list.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ExecutionLimitAddComponent } from './execution-limit-add/execution-limit-add.component';
import { ExecutionLimitEditComponent } from './execution-limit-edit/execution-limit-edit.component';
import { RoutesExecutionReason } from 'src/app/Shared/Routes/RoutesExecutionReasons';
import { RoutesExecutionLimit } from 'src/app/Shared/Routes/RoutesExecutionLimit';


const routes: Routes = [
  {
    path: '',
    children: [
      { path : RoutesExecutionLimit.MinimumExecutionLimit , component: ExecutionLimitListComponent },
      { path : RoutesExecutionLimit.NewExecutionLimit, component: ExecutionLimitAddComponent },
      { path : `${RoutesExecutionLimit.EditExecutionLimit}/:id`, component: ExecutionLimitEditComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExecutionLimitRoutingModule { }
