import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BaseLayoutComponent } from './LayoutComponent/base-layout/base-layout.component';
import { MsalGuard } from '@azure/msal-angular';
import { AuthGuard } from '../Shared/Guards/auth.guard';
import { RoutesLaout } from '../Shared/Routes/RoutesLaout';

const routes: Routes = [
    {
        path: '',
        component: BaseLayoutComponent,
        canActivate: [ MsalGuard, AuthGuard],
        children: [

            { path: '', redirectTo: RoutesLaout.Dashboard },
            // { path: '', component: MenuListComponent,data: {extraParameter: 'dashboardsMenu'} },
            { path: 'login', loadChildren: () => import('../Outside-Layout/outside-layout.module').then(m => m.OutsideLayoutModule) },
            { path: 'menu', loadChildren: () => import('./menu-details/menu-details.module').then(m => m.MenuDetailsModule) },   
            // tslint:disable-next-line:max-line-length
            { path: RoutesLaout.Question, loadChildren: () => import('./question-details/question-details.module').then(m => m.QuestionDetailsModule) },
            { path: 'product', loadChildren: () => import('./product-details/product-details.module').then(m => m.ProductDetailsModule) },
            // tslint:disable-next-line:max-line-length
            { path: 'posm-product', loadChildren: () => import('./posm-product-details/posm-product-details.module').then(m => m.PosmProductDetailsModule) },
            { path: 'role', loadChildren: () => import('./role-details/role-details.module').then(m => m.RoleDetailsModule) },
            { path: 'demo', loadChildren: () => import('./DemoPages/demo.module').then(m => m.DemoModule) },
            { path: 'users', loadChildren: () => import('./cm-user-details/user-details.module').then(m => m.UserDetailsModule) },
            { path: 'posm-product', loadChildren: () => import('./posm-product-details/posm-product-details.module').then(m => m.PosmProductDetailsModule) },
            { path: RoutesLaout.Task, loadChildren: () => import('./daily-activity/daily-activity.module').then(m => m.DailyActivityModule) },
            { path: 'users-info', loadChildren: () => import('./user-info/user-info.module').then(m => m.UserInfoModule) },
            { path: 'task', loadChildren: () => import('./daily-activity/daily-activity.module').then(m => m.DailyActivityModule) },
            { path: 'daily-posm', loadChildren: () => import('./daily-posm/daily-posm.module').then(m => m.DailyPosmModule) },
            { path: 'daily-audit', loadChildren: () => import('./daily-audit/daily-audit.module').then(m => m.DailyAuditModule) },
            { path: 'user', loadChildren: () => import('./azure-ad/azure-ad.module').then(m => m.AzureAdModule) },
            { path: 'work-flow', loadChildren: () => import('./work-flow/work-flow.module').then(m => m.WorkFlowModule) },
            { path: 'notification', loadChildren: () => import('./notification/notification.module').then(m => m.NotificationModule) },
            { path: RoutesLaout.Dashboard, loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule) },
            { path: 'access-denied', loadChildren: () => import('./access-denied/access-denied.module').then(m => m.AccessDeniedModule) },
            { path: 'campaign', loadChildren: () => import('./campaign-details/campaign-details.module').then(m => m.CampaignDetailsModule) },
            { path: 'brand', loadChildren: () => import('./brand-details/brand-details.module').then(m => m.BrandDetailsModule) },
            { path: 'subbrand', loadChildren: () => import('./subbrand-details/subbrand-details.module').then(m => m.SubBrandDetailsModule) },
            { path: 'configuration/execution-reasons', loadChildren: () => import('./execution-reasons/execution-reasons.module').then(m => m.ExecutionReasonsModule) },
            { path: RoutesLaout.Inventory, loadChildren: () => import('./inventory-management/inventory-management.module').then(m => m.InventoryManagementModule) },
            { path: RoutesLaout.AvCommunication, loadChildren: () => import('./av-communication/av-communication.module').then(m => m.AvCommunicationModule) },
            { path: RoutesLaout.AuditSetup, loadChildren: () => import('./audit-setup/audit-setup.module').then(m => m.AuditSetupModule) },
            { path: RoutesLaout.Guideline, loadChildren: () => import('./guideline/guideline.module').then(m => m.GuidelineModule) },      
            { path: RoutesLaout.Configuration, loadChildren: () => import('./execution-limit/execution-limit.module').then(m => m.ExecutionLimitModule) },      
            { path: RoutesLaout.HHTTaskReport, loadChildren: () => import('./hht-task-report/hht-task-report.module').then(m => m.HhtTaskReportModule) },      
            { path: RoutesLaout.Reports, loadChildren: () => import('./reports/reports.module').then(m => m.ReportsModule) },      
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LayoutRoutingModule { }
