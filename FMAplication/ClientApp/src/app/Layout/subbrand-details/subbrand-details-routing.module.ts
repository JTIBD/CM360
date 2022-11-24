import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SubBrandListComponent } from './subbrand-list/subbrand-list.component';
import { SubBrandAddComponent } from './subbrand-add/subbrand-add.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'subbrand-list' },
            { path: 'subbrand-list', component: SubBrandListComponent, canActivate: [PermissionGuard], data: { extraParameter: 'subbrand', permissionType: 'view', permissionGroup: 'subbrand/subbrand-list' } },
            { path: 'subbrand-add', component: SubBrandAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: 'subbrand/subbrand-list' } },
            { path: "subbrand-add/:id", component: SubBrandAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: 'subbrand/subbrand-list' } }
        ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SubBrandDetailsRoutingModule { }