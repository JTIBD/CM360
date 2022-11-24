import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { ExecutionReasonAddComponent } from './execution-reason-add/execution-reason-add.component';
import { ExecutionReasonListComponent } from './execution-reason-list/execution-reason-list.component';


const routes: Routes = [
	{
		path: '',
		children: [
			{ path: '', redirectTo: 'execution-reason-list'},
			{ path: 'execution-reason-list', component: ExecutionReasonListComponent, canActivate: [PermissionGuard], data: { permissionType: 'view', permissionGroup: 'configuration/execution-reasons'} },
			{ path: 'execution-reason-add', component:ExecutionReasonAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: 'configuration/execution-reasons'} },
			{ path: 'execution-reason-add/:id', component: ExecutionReasonAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: 'configuration/execution-reasons'} } 
		]
	}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExecutionReasonsRoutingModule { }
