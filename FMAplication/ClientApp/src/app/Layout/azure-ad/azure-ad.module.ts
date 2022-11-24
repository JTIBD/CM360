import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AzureAdRoutingModule } from './azure-ad-routing.module';
import { UserListComponent } from './user-list/user-list.component';
import { AzureAdComponent } from './azure-ad.component';
import { MsalModule, MsalInterceptor } from '@azure/msal-angular';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

export const protectedResourceMap: [string, string[]][] = [
    ['https://graph.microsoft.com/v1.0/me', ['user.read']]
    
];

const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;



@NgModule({
    declarations: [
        UserListComponent,
        AzureAdComponent
    ],
    imports: [
        CommonModule,
        AzureAdRoutingModule,
    //    MsalModule.forRoot({
    //        auth: {
    //            clientId: 'cab5412f-46b7-40f1-a096-93b91de6cb39',
    //            authority: 'https://login.microsoftonline.com/04900ed8-19fc-43dc-95ee-1f02b224607f/',
    //            validateAuthority: true,
    //            redirectUri: 'http://localhost:4200/',
    //            postLogoutRedirectUri: 'http://localhost:4200/',
    //            navigateToLoginRequestUrl: true,
    //        },
    //        cache: {
    //            cacheLocation: 'localStorage',
    //            storeAuthStateInCookie: isIE, // set to true for IE 11
    //        },
    //    },
    //        {
    //            popUp: false,
    //            consentScopes: [
    //                'user.read',
    //                'user.read.all',
    //                'group.read.all',
    //                'openid',
    //                'profile',
    //                'api://cab5412f-46b7-40f1-a096-93b91de6cb39/scope'
    //            ],
    //            unprotectedResources: ['https://www.microsoft.com/en-us/'],
    //            protectedResourceMap,
    //            extraQueryParameters: {}
    //        })
    //],
    //providers: [{
    //    provide: HTTP_INTERCEPTORS,
    //    useClass: MsalInterceptor,
    //    multi: true
    //}]
]
})
export class AzureAdModule { }
