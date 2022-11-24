import { AddCommunicationSetupComponent } from './add-communication-setup/add-communication-setup.component';
import { CommunicationSetupComponent } from './communication-setup/communication-setup.component';
import { AddAvCommunicationComponent } from './add-av-communication/add-av-communication.component';
import { AvCommunicationListComponent } from './av-communication-list/av-communication-list.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AvSetupComponent } from './av-setup/av-setup.component';
import { NewAvSetupComponent } from './new-av-setup/new-av-setup.component';
import { EditAvSetupComponent } from './edit-av-setup/edit-av-setup.component';
import { EditCommunicationSetupComponent } from './edit-communication-setup/edit-communication-setup.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { RoutesLaout } from 'src/app/Shared/Routes/RoutesLaout';
import { RoutesAvCommunication } from 'src/app/Shared/Routes/RoutesAvCommunication';


const routes: Routes = [
  {
    path: '',
    children: [

    { path:RoutesAvCommunication.AvCommunicationList, component: AvCommunicationListComponent, canActivate: [PermissionGuard], data: { permissionType: "view", permissionGroup: `${RoutesLaout.AvCommunication}/${RoutesAvCommunication.AvCommunicationList}`} },
    { path:RoutesAvCommunication.AddAvCommunication, component: AddAvCommunicationComponent, canActivate: [PermissionGuard], data: { permissionType: "create", permissionGroup: `${RoutesLaout.AvCommunication}/${RoutesAvCommunication.AvCommunicationList}`} },
    { path:`${RoutesAvCommunication.AddAvCommunication}/:id`, component: AddAvCommunicationComponent, canActivate: [PermissionGuard], data: { permissionType: "update", permissionGroup: `${RoutesLaout.AvCommunication}/${RoutesAvCommunication.AvCommunicationList}`} },
    { path:`${RoutesAvCommunication.EditCommunicationSetup}/:id`, component: EditCommunicationSetupComponent, canActivate: [PermissionGuard], data: { permissionType: "update", permissionGroup: `${RoutesLaout.AvCommunication}/${RoutesAvCommunication.CommunicationSetup}`} },
    { path:RoutesAvCommunication.AvSetup, component: AvSetupComponent, canActivate: [PermissionGuard], data: { permissionType: "view", permissionGroup: `${RoutesLaout.AvCommunication}/${RoutesAvCommunication.AvSetup}`} },
    { path:RoutesAvCommunication.NewAvSetup, component: NewAvSetupComponent, canActivate: [PermissionGuard], data: { permissionType: "create", permissionGroup: `${RoutesLaout.AvCommunication}/${RoutesAvCommunication.AvSetup}`} },
    { path:`${RoutesAvCommunication.EditAvSetup}/:id`, component: EditAvSetupComponent, canActivate: [PermissionGuard], data: { permissionType: "update", permissionGroup: `${RoutesLaout.AvCommunication}/${RoutesAvCommunication.AvSetup}`} },
    { path:RoutesAvCommunication.CommunicationSetup, component: CommunicationSetupComponent, canActivate: [PermissionGuard], data: { permissionType: "view", permissionGroup: `${RoutesLaout.AvCommunication}/${RoutesAvCommunication.CommunicationSetup}`} },
    { path:RoutesAvCommunication.AddCommunicationSetup, component: AddCommunicationSetupComponent, canActivate: [PermissionGuard], data: { permissionType: "create", permissionGroup: `${RoutesLaout.AvCommunication}/${RoutesAvCommunication.CommunicationSetup}`} }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AvCommunicationRoutingModule { }