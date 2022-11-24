import { Component } from '@angular/core';
import { BroadcastService, MsalService } from '@azure/msal-angular';
import { Logger, CryptoUtils } from 'msal';
import { AzureadService } from './Shared/Services/Azure/azuread.service';
import { LoginService } from './Shared/Services/Users/login.service';
import { Router } from '@angular/router';
import { environment } from '../environments/environment.prod';
import { ActivityPermissionService } from './Shared/Services/Activity-Permission/activity-permission.service';

const requestObj = {
    scopes: ['user.read']
};


@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
})

export class AppComponent {
    title = 'ArchitectUI - Angular 7 Bootstrap 4 & Material Design Admin Dashboard Template';
    profile;
    isIframe = false;
    loggedIn = false;
    public adTokenFromCache = false;
    adtoken: any;
    constructor(
        private loginService: LoginService,
        private authService: MsalService,
        private adService: AzureadService,
        private activityermissionService: ActivityPermissionService,
        private router: Router
    ) { }

    // tslint:disable-next-line:use-life-cycle-interface
    ngOnInit(): void {
        this.authService.handleRedirectCallback((authError, response) => {
            
            if (authError) {
              console.error('Redirect Error: ', authError.errorMessage);
              return this.router.navigate(['/login/unauthorized']);
            }
      
            console.log('Redirect Success: ', response);
        });
//  // tslint:disable-next-line:only-arrow-functions
//  this.authService.acquireTokenSilent(requestObj).then((tokenResponse) => {
//     localStorage.setItem('adtoken', tokenResponse.accessToken);
//     this.adtoken = tokenResponse.accessToken;
//     this.adTokenFromCache = tokenResponse.fromCache;
//     this.getProfile();
//     // tslint:disable-next-line:only-arrow-functions
// }).catch(function (error) {
//     return this.router.navigate(['/login/unauthorized']);
// });
       

          this.activityermissionService.setActivityPermissionToSession();
                
    }

    checkoutAccount() {
        this.loggedIn = !!this.authService.getAccount();
    }

    getProfile() {
        this.adService.getProfile()
            .toPromise().then(profile => {
                this.profile = profile;

                if (!this.adTokenFromCache || localStorage.getItem('fmapptoken') == null) {
                    this.login()
                }

            }).catch(err => {
                console.log(err);
                if (err.status == 401) {
                    this.getProfile()
                }
            });
    }

    login(): any {

        const loginModel = '{ "AdGuid" : "' + this.profile.id + '" , "email": "' + this.profile.givenName + '" }';
        const credentials = loginModel;
        this.loginService.Login(credentials).subscribe(
            (response: any) => {
                if (response.data.result !== null) {
                    var data = response.data.result;
                    const token = data.token;
                    localStorage.setItem('fmapptoken', token);
                    localStorage.setItem('fmapptoken.expiry', data.expiration);
                    this.loginService.getAdUser().subscribe((res: any) => {
                        console.log("User Data.......+++", res.data);
                        localStorage.setItem('userinfo', JSON.stringify(res.data));

                    })
                    return this.router.navigate['/'];
                }
                else {
                    return this.router.navigate(['/login/unauthorized']);
                }

            },
            error => {

                this.authService.clearCacheForScope(this.adtoken);
                localStorage.clear();
                return this.router.navigate(['/login/unauthorized']);

            }

        );
    }

   

    //login() {
    //    const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

    //    if (!isIE) {
    //        this.authService.loginRedirect();
    //    } else {
    //        this.authService.loginRedirect();
    //    }
    //}

    logout() {
        this.authService.logout();
    }

}
