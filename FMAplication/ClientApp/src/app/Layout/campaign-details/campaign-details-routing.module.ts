import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CampaignListComponent } from './campaign-list/campaign-list.component';
import { CampaignAddComponent } from './campaign-add/campaign-add.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { CampaignDetailsComponent } from './campaign-details/campaign-details.component';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'campaign-list' },
            { path: 'campaign-list', component: CampaignListComponent, canActivate: [PermissionGuard], data: { extraParameter: 'campaign', permissionType: 'view', permissionGroup: 'campaign/campaign-list' } },
            { path: 'campaign-add', component: CampaignAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: 'campaign/campaign-list' } },
            { path: "campaign-add/:id", component: CampaignAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: 'campaign/campaign-list' } },
            { path: "campaign-details/:id", component: CampaignDetailsComponent, canActivate: [PermissionGuard], data: { permissionType: 'view', permissionGroup: 'campaign/campaign-list' } }
        ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CampaignDetailsRoutingModule { }