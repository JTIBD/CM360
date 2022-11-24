import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { PermissionGuard } from "src/app/Shared/Guards/permission.guard";
import { RoutesLaout } from "src/app/Shared/Routes/RoutesLaout";
import { AuditSetupComponent } from "./audit-setup/audit-setup.component";
import { EditAuditSetupComponent } from "./edit-audit-setup/edit-audit-setup.component";
import { NewAuditSetupComponent } from "./new-audit-setup/new-audit-setup.component";
import { RoutesAudit } from "./routesAudit";


const routes: Routes = [
    {
        path: '',
        children: [            
            { path: RoutesAudit.list, component: AuditSetupComponent, canActivate: [PermissionGuard], data: { permissionType: 'view', permissionGroup: `${RoutesLaout.AuditSetup}/${RoutesAudit.list}`}  },            
            { path: RoutesAudit.create, component: NewAuditSetupComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: `${RoutesLaout.AuditSetup}/${RoutesAudit.list}`} },            
            { path: `${RoutesAudit.edit}/:id`, component: EditAuditSetupComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: `${RoutesLaout.AuditSetup}/${RoutesAudit.list}`} },            
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AuditSetupRoutingModule { }