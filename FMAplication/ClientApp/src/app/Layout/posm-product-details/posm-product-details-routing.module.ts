import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PosmProductListComponent } from './posm-product-list/posm-product-list.component';
import { PosmProductAddComponent } from './posm-product-add/posm-product-add.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'posm-product-list' },
            { path: 'posm-product-list', component: PosmProductListComponent, canActivate: [PermissionGuard], data: { extraParameter: 'product', permissionType: 'view', permissionGroup: 'posm-product/posm-product-list' } },
            { path: 'posm-product-add', component: PosmProductAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: 'posm-product/posm-product-list' } },
            { path: "posm-product-add/:id", component: PosmProductAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: 'posm-product/posm-product-list' } }
        ]
    }
]; 

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PosmProductDetailsRoutingModule { }
