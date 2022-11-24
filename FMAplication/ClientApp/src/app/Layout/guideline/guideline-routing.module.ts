import { EditGuidelineSetupComponent } from './edit-guideline-setup/edit-guideline-setup.component';
import { NewGuidelineSetupComponent } from './new-guideline-setup/new-guideline-setup.component';
import { GuidelineSetupComponent } from './guideline-setup/guideline-setup.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { RoutesGuidelineModule } from 'src/app/Shared/Routes/RoutesGuidelineModule';

export const GuidelineConstants = {
  GuidelineSetup: 'guideline-setup'
};

const routes: Routes = [
  {
    path: '',
    children: [
      { path : RoutesGuidelineModule.GuidelineSetup, component: GuidelineSetupComponent, canActivate: [PermissionGuard], data: { permissionType: "view", permissionGroup: `${RoutesLaout.Guideline}/${RoutesGuidelineModule.GuidelineSetup}` } },
      { path : RoutesGuidelineModule.NewGuidelineSetup, component: NewGuidelineSetupComponent, canActivate: [PermissionGuard], data: { permissionType: "create", permissionGroup: `${RoutesLaout.Guideline}/${RoutesGuidelineModule.GuidelineSetup}` }},
      { path : `${RoutesGuidelineModule.EditGuidelineSetup}/:id`, component: EditGuidelineSetupComponent, canActivate: [PermissionGuard], data: { permissionType: "update", permissionGroup: `${RoutesLaout.Guideline}/${RoutesGuidelineModule.GuidelineSetup}` }},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GuidelineRoutingModule { }
