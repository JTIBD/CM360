import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgReduxModule } from '@angular-redux/store';
import { NgRedux, DevToolsExtension } from '@angular-redux/store';
import { rootReducer, ArchitectUIState } from './ThemeOptions/store';
import { ConfigActions } from './ThemeOptions/store/config.actions';
import { AppRoutingModule } from './app-routing.module';
import { LoadingBarRouterModule } from '@ngx-loading-bar/router';

import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppComponent } from './app.component';

// BOOTSTRAP COMPONENTS

import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { AppInterceptorService } from './app-interceptor.service';

import { MsalModule, MsalInterceptor } from '@azure/msal-angular';
import { environment } from 'src/environments/environment';
import { AuthGuard } from './Shared/Guards/auth.guard';
import { PermissionGuard } from './Shared/Guards/permission.guard';



const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
    suppressScrollX: true
};

export const protectedResourceMap: [string, string[]][] = [
    ['https://graph.microsoft.com/v1.0/me', ['user.read']]
];

const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent
.indexOf('Trident/') > -1;

@NgModule({
    declarations: [


        AppComponent,


     


      
  ],
 imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        NgReduxModule,
        CommonModule,
        LoadingBarRouterModule,

        // Angular Bootstrap Components

        PerfectScrollbarModule,
        NgbModule,
        AngularFontAwesomeModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        MsalModule.forRoot({
            auth: {
                clientId: environment.clientId,
                authority: environment.authority,
                validateAuthority: true,
                redirectUri: environment.redirectUri,
                postLogoutRedirectUri: environment.postLogoutRedirectUri,
                navigateToLoginRequestUrl: true,
            },
            cache: {
                cacheLocation: 'localStorage',
                storeAuthStateInCookie: isIE, // set to true for IE 11
            },
        },
            {
                popUp: false,
                consentScopes: [
                    'user.read',
                    'openid',
                    'profile',                    
                ],
                unprotectedResources: ['https://www.microsoft.com/en-us/'],
                protectedResourceMap,
                extraQueryParameters: {}
            })
    ],
    providers: [{
        provide: HTTP_INTERCEPTORS,
        useClass: AppInterceptorService,
        multi: true
    },
    {
        provide:
            PERFECT_SCROLLBAR_CONFIG,
        // DROPZONE_CONFIG,
        useValue:
            DEFAULT_PERFECT_SCROLLBAR_CONFIG,
        // DEFAULT_DROPZONE_CONFIG,
    },
        ConfigActions,
        AuthGuard,
        PermissionGuard
    ],
    bootstrap: [AppComponent]
})

export class AppModule {
    constructor(private ngRedux: NgRedux<ArchitectUIState>,
        private devTool: DevToolsExtension) {

        this.ngRedux.configureStore(
            rootReducer,
            {} as ArchitectUIState,
            [],
            [devTool.isEnabled() ? devTool.enhancer() : f => f]
        );

    }
}
