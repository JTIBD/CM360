import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BrandListComponent } from './brand-list/brand-list.component';
import { BrandAddComponent } from './brand-add/brand-add.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'brand-list' },
            { path: 'brand-list', component: BrandListComponent, canActivate: [PermissionGuard], data: { extraParameter: 'brand', permissionType: 'view', permissionGroup: 'brand/brand-list' } },
            { path: 'brand-add', component: BrandAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: 'brand/brand-list' } },
            { path: "brand-add/:id", component: BrandAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: 'brand/brand-list' } }
        ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BrandDetailsRoutingModule { }