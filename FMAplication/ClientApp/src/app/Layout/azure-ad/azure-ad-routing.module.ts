import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserListComponent } from '../azure-ad/user-list/user-list.component';
import { AzureAdComponent } from './azure-ad.component';

const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'user-list' },
            { path: 'user-list', component: UserListComponent, data: { extraParameter: 'user' } },
            { path: 'ad-home', component: AzureAdComponent }
        ]
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AzureAdRoutingModule { }
